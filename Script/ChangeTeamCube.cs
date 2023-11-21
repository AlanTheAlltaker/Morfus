using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class ChangeTeamCube : NetworkBehaviour, IInteractable
{
    [SerializeField] Material red;
    [SerializeField] Material blue;
    public void Interact()
    {
        ChangeColorServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeColorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            ChangeMaterialClientRpc(player);
        }
    }
    [ClientRpc]
    public void ChangeMaterialClientRpc(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject player))
        {
            Renderer playerRenderer = player.GetComponent<Renderer>();
            if (player.gameObject.tag == "PlayerBlue")
            {
                player.gameObject.tag = "PlayerRed";
                player.gameObject.layer = 8;
                playerRenderer.material = red;
            }
            else if (player.gameObject.tag == "PlayerRed")
            {
                player.gameObject.tag = "PlayerBlue";
                player.gameObject.layer = 9;
                playerRenderer.material = blue;
            }
        }
    }
}
