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
            TagsManager tagsManager = player.GetComponent<TagsManager>();
            if (tagsManager.tagsList.Contains("Blue") && !tagsManager.tagsList.Contains("Red"))
            {
                tagsManager.tagsList.Add("Red");
                tagsManager.tagsList.Remove("Blue");
                Debug.Log("Dodano Red dla gracza");

                playerRenderer.material = red;
            }
            else if (tagsManager.tagsList.Contains("Red") && !tagsManager.tagsList.Contains("Blue"))
            {
                tagsManager.tagsList.Add("Blue");
                tagsManager.tagsList.Remove("Red");
                Debug.Log("Dodano Blue dla gracza");

                playerRenderer.material = blue;
            }
        }
    }
}
