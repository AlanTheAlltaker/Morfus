using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class explosionAudio : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DestroyServerRpc();
    }
    [ServerRpc (RequireOwnership = false)]
    void DestroyServerRpc()
    {
        if (!IsServer) return;
        Destroy(gameObject, 0.5f);
    }
}
