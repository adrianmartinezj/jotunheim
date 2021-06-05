using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanoidLocomotionController))]
public class PlayerController : MonoBehaviour
{
    private HumanoidLocomotionController m_locomotionController;
    private Vector3 m_movementDirection;
    private bool m_isGrounded = false;
    private float m_groundTolerance = .2f;

    private const float JUMP_DELAY_TIME = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        m_locomotionController = GetComponent<HumanoidLocomotionController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Grab input from player
        ListenForInput();
    }

    private void ListenForInput()
    {
        // Zero out all previous directions

        if (Input.GetAxis("Horizontal") != 0)
        {
            m_movementDirection.x = Input.GetAxis("Horizontal");
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            m_movementDirection.z = Input.GetAxis("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_isGrounded)
            {
                m_locomotionController.Jump();
                m_isGrounded = false;
            }
        }

        if (Input.GetAxis("Sprint") != 0)
        {
            m_locomotionController.isSprinting = true;
        }
        else
        {
            m_locomotionController.isSprinting = false;
        }

        if (m_movementDirection != Vector3.zero)
        {
            m_locomotionController.MoveTowards(m_movementDirection.normalized);
            m_movementDirection = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2, 0), Vector3.down, out RaycastHit hit))
        {
            if (hit.distance < m_groundTolerance)
            {
                m_isGrounded = true;
            }
            else
            {
                m_isGrounded = false;
            }
        }
    }
}
