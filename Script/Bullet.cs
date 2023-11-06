using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float life = 2f;
    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] float bouncingHeight = 2f;

    bool dealtDamage = false;


    void Awake()
    {
        Invoke(nameof(Destroy), 3f);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;   
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed + this.transform.up * bouncingHeight;
    }
    public void Destroy()
    {
        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
}