using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CannonBullet : NetworkBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] AudioSource flyingBullet;

    [SerializeField] GameObject explosionAudio;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        explosion.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.up * bulletSpeed;
        TurnOnAudioServerRpc();
    }

    void OnCollisionEnter(Collision collision)
    {
        explosion.SetActive(true);
        ShootServerRpc(transform.position, transform.rotation);
        Invoke(nameof(DestroyServerRpc), 0.05f);
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
    [ServerRpc(RequireOwnership = false)]
    void TurnOnAudioServerRpc()
    {
        TurnAudioOnClientRpc();

    }
    [ClientRpc]
    void TurnAudioOnClientRpc()
    {
        flyingBullet.Play();
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(explosionAudio, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
    // DESPAWN OBJECT
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
