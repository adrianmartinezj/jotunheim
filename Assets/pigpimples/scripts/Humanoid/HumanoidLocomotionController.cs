using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HumanoidLocomotionController : MonoBehaviour
{
    private float m_baseSpeed = 80.0f;
    private float m_sprintSpeed = 120.0f;
    private float m_baseRotationSpeed = 4000f;
    private float m_jumpHeight = 5f;
    public bool isSprinting = false;
    private Rigidbody m_rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ?
    /// </summary>
    /// <param name="direction">Normalized unit vector representing 
    /// the current direction for this humanoid to move towards.</param>
    public void MoveTowards(Vector3 direction)
    {
        // Literally move the humanoid
        Vector3 curPos = transform.position;
        float speedMultiplier = ((isSprinting ? m_sprintSpeed : m_baseSpeed) * Time.deltaTime);
        Vector3 newPos = curPos + (direction * speedMultiplier);
        m_rigidBody.MovePosition(newPos);

        // Start rotating towards that direction
        //float alpha = EaseFunctions.EaseInOutCubic((1 - (Vector3.Angle(transform.forward, direction) / 180f)));
        //Vector3 newRotation = Vector3.Lerp(transform.forward, direction, EaseFunctions.EaseInOutCubic(alpha));
        //print("newRotation: " + newRotation);
        //m_rigidBody.MoveRotation(Quaternion.LookRotation(newRotation));

        m_rigidBody.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                Quaternion.LookRotation(direction), 
            (Time.deltaTime * m_baseRotationSpeed));
    }

    public void Jump()
    {
        m_rigidBody.AddForce(new Vector3(0, m_jumpHeight, 0), ForceMode.Impulse);
    }
}
