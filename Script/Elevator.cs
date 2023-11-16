using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Elevator : NetworkBehaviour
{
    [SerializeField]
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name == "Player(Clone)")
        {
            Rigidbody rB = collision.GetComponent<Rigidbody>();
            rB.AddForce(rB.transform.up * 10f);
        }
    }
}
