using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

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
    public bool isGrounded;

    //Footsteps
    [SerializeField] AudioSource footsteps;
    bool walking = false;

    void Update()
    {
        if (!IsOwner) return;
        WSAD();
        Jump();
        Footsteps();
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

    void Footsteps()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !walking && isGrounded)
        {
            EnableFootstepsServerRpc(true);
            Debug.Log("Odpalam");
            walking = true;
        } else if (!isGrounded && walking)
        {
            EnableFootstepsServerRpc(false);
            Debug.Log("Gasze");
            walking = false;
        } else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && walking)
        {
            EnableFootstepsServerRpc(false);
            Debug.Log("Gasze");
            walking = false;
        }
    }

    // ENABLE FOOTSTEPS
    [ServerRpc(RequireOwnership = false)]
    void EnableFootstepsServerRpc(bool isWhat)
    {
        EnableFootstepsClientRpc(isWhat);
    }
    [ClientRpc]
    void EnableFootstepsClientRpc(bool isWhat)
    {
        footsteps.enabled = isWhat;
    }
}