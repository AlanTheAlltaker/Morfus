using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float life = 2f;
    [SerializeField] float bulletSpeed = 2f;


    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0 )
        {
            DestroyServerRpc();
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;   
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player(Clone)")
        {
            DestroyServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
       gameObject.GetComponent<NetworkObject>().Despawn();
    }
}