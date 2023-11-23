using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    [SerializeField] GameObject cameraHolder;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        BuildPlayerServerRpc();
    }
    [ServerRpc (RequireOwnership = false)]
    void BuildPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            GameObject bullet = Instantiate(cameraHolder, transform.position, transform.rotation);
            bullet.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            Transform player = client.PlayerObject.transform;
            bullet.transform.SetParent(player);
        }
    }
}
