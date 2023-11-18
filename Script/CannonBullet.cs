using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CannonBullet : NetworkBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float bulletSpeed = 2f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        explosion.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.up * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        explosion.SetActive(true);
        Invoke(nameof(Destroy), 0.05f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthDamage>())
        {
            HealthDamage healthDamage = other.GetComponent<HealthDamage>();
            if (healthDamage.isPlayer || healthDamage.isStructure)
            {
                if (!IsServer) return;
                healthDamage.healthPoint.Value -= 100;
            }
        }
    }
    public void Destroy()
    {
        if (IsServer)
        {
            Destroy(gameObject);
        }
    }
}
