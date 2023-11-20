using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Teleport : NetworkBehaviour
{
    [SerializeField] Transform teleportPoint;
    [SerializeField] GameObject spawnSound;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetParentServerRpc();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Movement>())
        {
            if (teleportPoint != null)
            {
                collision.transform.position = teleportPoint.position + transform.up * 2;
                ShootServerRpc(teleportPoint.position, teleportPoint.rotation);
            } else
            {
                Destroy(gameObject);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SetParentServerRpc()
    {
        SetParentClientRpc();
    }
    [ClientRpc]
    void SetParentClientRpc()
    {
        transform.SetParent(null);
    }
    // SPAWN SOUND
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(spawnSound, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
