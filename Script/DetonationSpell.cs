using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DetonationSpell : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float cooldown;

    [SerializeField] AudioSource beep;
    [SerializeField] GameObject explosionAudio;

    int nextUpdate = 1;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb.isKinematic = false;
        rb.velocity = transform.forward * 5f;
    }
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            cooldown -= 1;
            if (cooldown == 1)
            {
                beep.Stop();
                ShootServerRpc(transform.position, transform.rotation);
            }
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (cooldown <= 0)
        {
            if (collider.gameObject.GetComponent<HealthDamage>())
            {
                if (!IsServer) { return; }
                HealthDamage healthDamage = collider.gameObject.GetComponent<HealthDamage>();
                if (!healthDamage.isBoss)
                {
                    healthDamage.healthPoint.Value -= 100;
                }
                Destroy(gameObject);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(explosionAudio, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
