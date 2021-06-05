using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private CapsuleCollider m_collider;
    [SerializeField]
    private Rigidbody m_rigidBody;
    [SerializeField]
    private GameObject m_cameraBoom;

    private const float MOUSE_TURN_FACTOR = 7.5f;
    private const float MOUSE_PITCH_FACTOR = 1.25f;
    // 10mph about
    private const float MAX_FORWARD_VELOCITY = 4.4f;
    private const float MAX_HORIZONTAL_VELOCITY = 3.4f;
    private const float RAMP_UP_TIME = 0.5f;
    private const bool INVERT_MOUSE_Y = false;

    private Vector3 m_movementDir = Vector3.zero;
    private float m_movementSpeed = 0.060f;
    private float m_jumpSpeed = 300f;
    private float m_groundTolerance = 0.05f;
    private float m_heldTime;
    private bool m_isGrounded = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // apply player rotation for mouse input
        if (Input.GetAxis("Mouse X") != 0)
        {
            var curRot = transform.rotation;
            m_rigidBody.MoveRotation(Quaternion.Euler(new Vector3(
                curRot.eulerAngles.x,
                curRot.eulerAngles.y + (Input.GetAxis("Mouse X") * MOUSE_TURN_FACTOR),
                curRot.eulerAngles.z))
            );
        }

        // apply the player looking up or down
        if (Input.GetAxis("Mouse Y") != 0)
        {
            var curCamRot = m_cameraBoom.transform.rotation;
            m_cameraBoom.transform.rotation = Quaternion.Euler(new Vector3(
                    curCamRot.eulerAngles.x + (INVERT_MOUSE_Y ? 1 : -1 * Input.GetAxis("Mouse Y") * MOUSE_PITCH_FACTOR),
                    curCamRot.eulerAngles.y,
                    curCamRot.eulerAngles.z
                )
            );
        }

        // apply movement based on vector input
        if (Input.GetKey(KeyCode.W))
        {
            m_movementDir += transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_movementDir -= transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_movementDir -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_movementDir += transform.right;
        }
        if (m_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            m_rigidBody.AddForce(new Vector3(
                0,
                m_jumpSpeed,
                0
                ), ForceMode.Acceleration);
            m_animator.SetBool("isJumping", true);
        }

        if (m_movementDir != Vector3.zero)
        {
            // increment the time we've held a velocity key for
            m_heldTime += Time.deltaTime;
            // normalize movement vector
            m_movementDir.Normalize();
            // TODO: Add in ramp up / ramp down movement input system
            //var prevPos = transform.position;
            //var newPos = prevPos + new Vector3(
            //    m_movementDir.x * m_movementSpeed,
            //    0,
            //    m_movementDir.z * m_movementSpeed
            //);
            //m_rigidBody.MovePosition(newPos);
            m_rigidBody.AddForce(GetUpdatedVelocity(), ForceMode.Acceleration);
            //m_rigidBody.AddForce(new Vector3(
            //        m_movementDir.x * m_movementSpeed,
            //        0,
            //        m_movementDir.z * m_movementSpeed
            //    ), ForceMode.Impulse);
            // set it back to zero
            m_movementDir = Vector3.zero;
        }
        else
        {
            m_heldTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EditorApplication.isPlaying = false;
        }

        print("ForwardVelocity: " + m_rigidBody.velocity.x + " RightVelocity:" + m_rigidBody.velocity.z + " VerticalVelocity:" + m_rigidBody.velocity.y);
        m_animator.SetFloat("ForwardVelocity", m_rigidBody.velocity.x);
        m_animator.SetFloat("RightVelocity", m_rigidBody.velocity.z);
        m_animator.SetFloat("VerticalVelocity", m_rigidBody.velocity.y);
    }

    private Vector3 GetUpdatedVelocity()
    {
        Vector3 resultants = Vector3.zero;
        Vector3 anticipated = m_movementDir * m_movementSpeed;
        float CURRENT_FORWARD_VELOCITY = m_rigidBody.velocity.x;
        float CURRENT_HORIZONTAL_VELOCITY = m_rigidBody.velocity.z;
        // handle x (forward)
        if (Mathf.Abs(CURRENT_FORWARD_VELOCITY) < MAX_FORWARD_VELOCITY)
        {
            // CANT CHECK IT LIKE THIS
            // the signs get fucked up if the velocity is going the opposite direction
            // resulting in weird behavior from the directions when we start adding shit.
            // fix this
            var newForwardVelocity = 
                anticipated.x + CURRENT_FORWARD_VELOCITY <= MAX_FORWARD_VELOCITY 
                    ? anticipated.x
                    : MAX_FORWARD_VELOCITY - anticipated.x;
            resultants.x = newForwardVelocity;
        }

        // handle z (horizontal)
        if (Mathf.Abs(CURRENT_HORIZONTAL_VELOCITY) < MAX_HORIZONTAL_VELOCITY)
        {
            var newHorizontalVelocity =
                anticipated.z + CURRENT_HORIZONTAL_VELOCITY <= MAX_HORIZONTAL_VELOCITY
                    ? anticipated.z
                    : MAX_HORIZONTAL_VELOCITY - anticipated.z;
            resultants.z = newHorizontalVelocity;
        }
        //print("resultants: " + resultants);
        return resultants;
    }

    private void FixedUpdate()
    {
        // Grounded check constantly
        Physics.Raycast(new Ray(transform.position, -transform.up), out RaycastHit hitInfo);
        m_isGrounded = hitInfo.distance <= m_groundTolerance;
    }
}
