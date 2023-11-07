using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Building : NetworkBehaviour
{
    [SerializeField] GameObject sentryPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] Transform player;
    [SerializeField] BuildingBullet bB;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BuildWall()
    {
        BuildWallServerRpc();
    }
    public void BuildSentry()
    {
        BuildSentryServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void BuildSentryServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            BuildSentryClientRpc(player);
        }
    }
    [ClientRpc]
    public void BuildSentryClientRpc(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject player))
        {
            BuildingGun gunCannon = player.GetComponentInChildren<BuildingGun>();
            gunCannon.bulletPrefab = sentryPrefab;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void BuildWallServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            BuildWallClientRpc(player);
        }
    }
    [ClientRpc]
    public void BuildWallClientRpc(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject player))
        {
            BuildingGun gunCannon = player.GetComponentInChildren<BuildingGun>();
            gunCannon.bulletPrefab = wallPrefab;
        }
    }
}