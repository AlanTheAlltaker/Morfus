using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Flying : NetworkBehaviour
{
    public bool flyingEffect = false;
    [SerializeField] Movement move;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flyingEffect && !move.isGrounded)
        {
            rb.AddForce(player.forward * 1f);
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(player.up * 1.2f);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        flyingEffect = false;
    }
}
