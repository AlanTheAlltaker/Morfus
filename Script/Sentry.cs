using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Sentry : NetworkBehaviour
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float cooldownTime = 3f;
    bool isCooldown = false;

    bool playerInRange;
    [SerializeField] float sentryRange;
    [SerializeField] LayerMask whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, sentryRange, whatIsPlayer);


        if (IsServer && playerInRange && !isCooldown)
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
    [ServerRpc]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
