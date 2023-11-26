using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PickUpStructure : NetworkBehaviour, IInteractable
{
    public void Interact()
    {
        if (transform.parent == null)
        {
            PickUpServerRpc();
            Debug.Log("Picking");
        }
    }
    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.G) && transform.parent != null)
            {
                DropServerRpc();
                Debug.Log("Droping");
            }
        }
    }

    // ----------------------------------------> PICK UP ITEM <----------------------------------------
    [ServerRpc (RequireOwnership = false)]
    void PickUpServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            Transform player = client.PlayerObject.transform;
            Transform structureHolder = player.GetChild(1).GetChild(1).GetChild(1);
            Holder holder = player.GetChild(1).GetChild(1).GetComponent<Holder>();
            if (!holder.isStructureHolderFull)
            {
                holder.HolderServerRpc(false, true, false, false);
                transform.SetParent(structureHolder);
                GetComponent<NetworkObject>().ChangeOwnership(clientId);
                GetComponent<BuildingRay>().enabled = true;
                PickUpClientRpc(player.GetComponent<NetworkObject>());
            }
        }
    }
    [ClientRpc]
    void PickUpClientRpc(NetworkObjectReference playerRef)
    {
        playerRef.TryGet(out NetworkObject playerNet);
        Transform player = playerNet.GetComponent<Transform>();
        Holder holder = player.GetChild(1).GetChild(1).GetComponent<Holder>();
        Destroy(GetComponent<NetworkRigidbody>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<NetworkTransform>());
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        holder.isStructureHolderFull = true;
        GetComponent<BuildingRay>().enabled = true;
    }

    // ----------------------------------------> DROP ITEM <----------------------------------------
    [ServerRpc(RequireOwnership = false)]
    void DropServerRpc()
    {
        if (transform.parent != null)
        {
            Holder holder = transform.parent.transform.parent.GetComponent<Holder>();
            if (holder != null && holder.isStructureHolderFull)
            {
                DropClientRpc();
                transform.SetParent(null);
                GetComponent<NetworkObject>().RemoveOwnership();
            }
        }
    }
    [ClientRpc]
    void DropClientRpc()
    {
        gameObject.AddComponent<NetworkRigidbody>();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 5;
        Holder holder = transform.parent.transform.parent.GetComponent<Holder>();
        holder.isStructureHolderFull = false;
        GetComponent<BuildingRay>().enabled = false;
    }

}
