using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanoidLocomotionController))]
public class PlayerController : MonoBehaviour
{
    private HumanoidLocomotionController m_locomotionController;
    private Vector3 m_movementDirection;
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
            print("Input.GetAxis(Horizontal): " + Input.GetAxis("Horizontal"));
            m_movementDirection.x = Input.GetAxis("Horizontal");
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            m_movementDirection.z = Input.GetAxis("Vertical");
        }

        if (m_movementDirection != Vector3.zero)
        {
            m_locomotionController.MoveTowards(m_movementDirection.normalized);
            m_movementDirection = Vector3.zero;
        }
    }
}
