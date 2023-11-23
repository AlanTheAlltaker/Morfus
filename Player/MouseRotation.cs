using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MouseRotation : NetworkBehaviour
{
    Transform playerBody;
    [SerializeField] float mouseSensitivity = 600f;
    float xRotation = 0f;
    [SerializeField] GameObject cameraPlayer;

    // Start is called before the first frame update

    private void OnTransformParentChanged()
    {
        playerBody = transform.parent;
        transform.position = Vector3.zero + Vector3.up;
    }
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }
        cameraPlayer.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        MouseeRotation();
    }
    void MouseeRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}