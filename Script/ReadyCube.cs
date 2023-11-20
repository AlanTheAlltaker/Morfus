using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ReadyCube : NetworkBehaviour, IInteractable
{
    [SerializeField] float cooldownTime = 0.01f;
    [SerializeField] Transform redSpawner;
    [SerializeField] Transform blueSpawner;
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
        var script = player.GetComponent<Movement>();
        script.enabled = false;
        yield return new WaitForSeconds(cooldownTime);
        if (player.gameObject.tag == "PlayerBlue")
        {
            player.transform.position = blueSpawner.position;
        } else if (player.gameObject.tag == "PlayerRed")
        {
            player.transform.position = redSpawner.position;
        }
        yield return new WaitForSeconds(cooldownTime);
        script.enabled = true;
    }
}
