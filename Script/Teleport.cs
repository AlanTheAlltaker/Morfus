using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Teleport : NetworkBehaviour
{
    [SerializeField] Transform teleportPoint;

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
}
