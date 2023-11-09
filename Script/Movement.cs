using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

public class Movement : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
    }
    // WSAD
    [SerializeField] Transform playerBody;
    [SerializeField] int speed = 5;

    //JUMP
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] int jumpVelocity;
    public bool isGrounded;

    [SerializeField] BuildingGun buildingGun;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        WSAD();

        Jump();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var cameraScript = GetComponentInChildren<MouseRotation>();
            cameraScript.enabled = false;

            buildingGun.uiOpen = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            var cameraScript = GetComponentInChildren<MouseRotation>();
            cameraScript.enabled = true;

            buildingGun.uiOpen = false;
        }

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