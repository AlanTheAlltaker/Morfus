using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportationPoint : NetworkBehaviour, IInteractable
{
    [SerializeField] Transform teleportPoint;
    [SerializeField] Transform teleport;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    public void Interact()
    {
        SetParentServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void SetParentServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject target = client.PlayerObject.GetComponent<NetworkObject>();
            SetParentClientRpc(target);
        }
    }
    [ClientRpc]
    void SetParentClientRpc(NetworkObjectReference target)
    {
        target.TryGet(out NetworkObject playerNet);
        Transform player = playerNet.GetComponent<Transform>();
        if (teleportPoint.parent == null)
        {
            teleportPoint.SetParent(player);
        } else if (teleportPoint.parent != null)
        {
            teleportPoint.SetParent(null);
        }
    }
}
