using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ReadyCube : NetworkBehaviour, IInteractable
{
    [SerializeField] float cooldownTime = 0.01f;
    public void Interact()
    {
        ReadyPlayerServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void ReadyPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            ReadyPlayerClientRpc(player);
        }
    }
    [ClientRpc]
    public void ReadyPlayerClientRpc(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject player))
        {
            StartCoroutine(Cooldown(player));
        }
    }
    IEnumerator Cooldown(NetworkObject player)
    {
        TagsManager tagsManager = player.GetComponent<TagsManager>();
        var script = player.GetComponent<Movement>();
        script.enabled = false;
        yield return new WaitForSeconds(cooldownTime);
        if (tagsManager.tagsList.Contains("Blue"))
        {
            player.transform.position = new Vector3(-100f, 2f, 100f);
        } else if (tagsManager.tagsList.Contains("Red"))
        {
            player.transform.position = new Vector3(-100f, 2f, -100f);
        }
        yield return new WaitForSeconds(cooldownTime);
        script.enabled = true;
    }
}
