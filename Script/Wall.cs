using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Wall : NetworkBehaviour, IInteractable
{
    [SerializeField] BoxCollider wall;
    [SerializeField] BoxCollider wallFront;
    [SerializeField] BoxCollider wallBack;
    [SerializeField] BoxCollider wallLeft;
    [SerializeField] BoxCollider wallRight;
    [SerializeField] BoxCollider wallUp;
    [SerializeField] BoxCollider wallDown;

    [SerializeField] Material wallMaterial;
    [SerializeField] Material ghostMaterial;

    [SerializeField] Renderer wallRenderer;

    [SerializeField] bool isRed;
    [SerializeField] bool isBlue;


    public void Interact()
    {
        OpenDoorServerRpc();
    }
    IEnumerator TurnOn()
    {
        wallRenderer.material = ghostMaterial;
        wall.enabled = false;
        wallFront.enabled = false;
        wallBack.enabled = false;
        wallLeft.enabled = false;
        wallRight.enabled = false;
        wallUp.enabled = false;
        wallDown.enabled = false;
        yield return new WaitForSeconds(2f);
        wallRenderer.material = wallMaterial;
        wall.enabled = true;
        wallFront.enabled = true;
        wallBack.enabled = true;
        wallLeft.enabled = true;
        wallRight.enabled = true;
        wallUp.enabled = true;
        wallDown.enabled = true;
    }
    [ServerRpc(RequireOwnership = false)]
    public void OpenDoorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject target = client.PlayerObject;
            OpenDoorClientRpc(target);
        }
    }
    [ClientRpc]
    void OpenDoorClientRpc(NetworkObjectReference target)
    {
        target.TryGet(out NetworkObject player);
        if (isRed)
        {
            if (player.tag == "PlayerRed")
            {
                StartCoroutine(TurnOn());
            }
        }
        if (isBlue)
        {
            if (player.tag == "PlayerBlue")
            {
                StartCoroutine(TurnOn());
            }
        }
    }
}
