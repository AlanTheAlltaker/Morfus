using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlyingSpellItem : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 2;
    }
    PotionHolder potionHolder;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player(Clone)")
        {
            GameObject pH = collision.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
            potionHolder = pH.GetComponent<PotionHolder>();

            if (potionHolder.fP != true)
            {
                FPServerRpc(collision.gameObject.GetComponent<NetworkObject>());
                DestroyServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
        DestroyClientRpc();
    }
    [ClientRpc]
    void DestroyClientRpc()
    {
        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
    [ServerRpc (RequireOwnership = false)]
    void FPServerRpc(NetworkObjectReference target)
    {
        FPClientRpc(target);
    }
    [ClientRpc]
    void FPClientRpc(NetworkObjectReference target)
    {
        target.TryGet(out NetworkObject player);
        Transform playerTransform = player.GetComponent<Transform>();
        GameObject pH = playerTransform.GetChild(1).GetChild(2).GetChild(1).gameObject;
        potionHolder = pH.GetComponent<PotionHolder>();
        potionHolder.fP = true;
    }
}
