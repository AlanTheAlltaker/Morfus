using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class BuildingGun : NetworkBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    [SerializeField] float cooldownTime = 0f;
    bool isCooldown = false;

    public bool uiOpen = false;

    public NetworkVariable<int> tokens = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        tokens.Value = 0;
    }

    private void OnEnable()
    {
        StartCoroutine(Cooldown());
    }

    void Update()
    {
        if (!IsOwner) { return; }
        if (Input.GetMouseButtonDown(0) && !isCooldown && !uiOpen)
        {
            StartCoroutine(Cooldown());
            if (bulletPrefab.name == "WallBuilder")
            {
                ShootServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation, 1);
                Debug.Log("Posiadasz " + tokens.Value);
            }
            if (bulletPrefab.name == "SentryBuilder")
            {
                ShootServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation, 3);
                Debug.Log("Posiadasz " + tokens.Value);
            }

        }
    }
    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation, int charge)
    {
        if (tokens.Value >= charge)
        {
            GameObject bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<NetworkObject>().Spawn();
            tokens.Value -= charge;
        }
    }
}
