using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AudioObject : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Invoke(nameof(DestroyServerRpc), 1f);
    }

    // DESPAWN OBJECT
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
