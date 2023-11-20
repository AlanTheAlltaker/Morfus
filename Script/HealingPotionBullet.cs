using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealingPotionBullet : NetworkBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb.isKinematic = false;
        rb.velocity = transform.forward * 10f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        ShootServerRpc(transform.position, transform.rotation);
        if (!IsServer) return;
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
