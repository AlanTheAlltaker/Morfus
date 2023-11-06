using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GunCannon : NetworkBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    [SerializeField] float cooldownTime = 0f;
    bool isCooldown = false;

    void Update()
    {
        if (!IsOwner) { return; }
        if (Input.GetMouseButtonDown(0) && !isCooldown)
        {
            StartCoroutine(Cooldown());
            ShootServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
    [ServerRpc (RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}