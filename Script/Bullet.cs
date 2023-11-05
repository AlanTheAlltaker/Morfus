using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float life = 2f;
    [SerializeField] float bulletSpeed = 2f;

    bool dealtDamage = false;


    void Awake()
    {
        Invoke(nameof(DestroyServerRpc), 3f);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;   
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HealthDamage>() && !dealtDamage)
        {
            //collision.gameObject.GetComponent<HealthDamage>().hp -= 20f;
            //Debug.Log("pozosta³o " + collision.gameObject.GetComponent<HealthDamage>().hp);
            //dealtDamage = true;
            //Invoke(nameof(DestroyServerRpc), 0.15f);
            //DestroyServerRpc();
        }
        //GetComponent<Rigidbody>().velocity = this.transform.up + this.transform.forward * bulletSpeed;
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
       gameObject.GetComponent<NetworkObject>().Despawn();
    }
}