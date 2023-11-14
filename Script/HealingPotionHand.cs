using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class HealingPotionHand : NetworkBehaviour
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] PotionHolder potionHolder;
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButton(0))
        {
            ShootServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            FPServerRpc();
            HideServerRpc();
        }
    }

    // Shoot
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    //Set false
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        potionHolder.hP = false;
    }

    //Hide
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc()
    {
        HideClientRpc();
    }
    [ClientRpc]
    void HideClientRpc()
    {
        gameObject.SetActive(false);
    }
}
