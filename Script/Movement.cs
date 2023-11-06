using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    // WSAD
    [SerializeField] Transform playerBody;
    [SerializeField] int speed = 5;

    //JUMP
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] int jumpVelocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        WSAD();

        Jump();

    }
    void WSAD()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerBody.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerBody.transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerBody.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerBody.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
    }
    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpVelocity);
        }
    }
}