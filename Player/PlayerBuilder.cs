using Unity.Netcode;
using UnityEngine;


public class PlayerBuilder : NetworkBehaviour
{
    [SerializeField] GameObject cameraHolderItem;
    [SerializeField] GameObject itemHolderItem;
    [SerializeField] GameObject weaponHolderItem;
    [SerializeField] GameObject structureHolderItem;
    [SerializeField] GameObject spellHolderItem;
    [SerializeField] GameObject potionHolderItem;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            BuildPlayerServerRpc();
            transform.position = new Vector3(0, 10, 0);
            UpdatePlayerServerRpc();
            Debug.Log("Update");
        }
    }

    // ----------------------------------------> BUILD PLAYER <----------------------------------------
    [ServerRpc (RequireOwnership = false)]
    public void BuildPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            Transform player = client.PlayerObject.transform;

            // CAMERA HOLDER
            GameObject cameraHolder = Instantiate(cameraHolderItem, transform.position, transform.rotation);
            cameraHolder.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            cameraHolder.transform.SetParent(player);

            // ITEM HOLDER
            GameObject itemHolder = Instantiate(itemHolderItem, transform.position, transform.rotation);
            itemHolder.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            itemHolder.transform.SetParent(cameraHolder.transform);

            // WEAPON HOLDER
            GameObject weaponHolder = Instantiate(weaponHolderItem, transform.position, transform.rotation);
            weaponHolder.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            weaponHolder.transform.SetParent(itemHolder.transform);

            // STRUCTURE HOLDER
            GameObject structureHolder = Instantiate(structureHolderItem, transform.position, transform.rotation);
            structureHolder.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            structureHolder.transform.SetParent(itemHolder.transform);

            // SPELL HOLDER
            GameObject spellHolder = Instantiate(spellHolderItem, transform.position, transform.rotation);
            spellHolder.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            spellHolder.transform.SetParent(itemHolder.transform);

            // POTION HOLDER
            GameObject potionHolder = Instantiate(potionHolderItem, transform.position, transform.rotation);
            potionHolder.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            potionHolder.transform.SetParent(itemHolder.transform);

            //BUILD PLAYER
            BuildPlayerClientRpc(
                player.GetComponent<NetworkObject>(),
                cameraHolder.GetComponent<NetworkObject>(),
                itemHolder.GetComponent<NetworkObject>(), 
                weaponHolder.GetComponent<NetworkObject>(),
                structureHolder.GetComponent<NetworkObject>(),
                spellHolder.GetComponent<NetworkObject>(),
                potionHolder.GetComponent<NetworkObject>());
        }
    }
    [ClientRpc]
    void BuildPlayerClientRpc(
        NetworkObjectReference targetPlayer,
        NetworkObjectReference targetCamera,
        NetworkObjectReference target3,
        NetworkObjectReference targetWeaponHolder, 
        NetworkObjectReference targetStructureHolder,
        NetworkObjectReference targetSpellHolder,
        NetworkObjectReference targetPotionHolder)
    {
        // PLAYER
        targetPlayer.TryGet(out NetworkObject player);

        // CAMERA  HOLDER
        targetCamera.TryGet(out NetworkObject cameraHolderX);
        GameObject cameraHolder = cameraHolderX.gameObject;
        cameraHolder.GetComponent<MouseRotation>().playerBody = player.transform;
        cameraHolder.transform.localPosition = Vector3.zero + Vector3.up * 0.75f;

        // ITEM HOLDER
        target3.TryGet(out NetworkObject itemHolderX);
        GameObject itemHolder = itemHolderX.gameObject;
        itemHolder.transform.localPosition = Vector3.zero + Vector3.down * .75f;

        // WEAPON HOLDER
        targetWeaponHolder.TryGet(out NetworkObject weaponHolderX);
        GameObject weaponHolder = weaponHolderX.gameObject;
        weaponHolder.transform.localPosition = Vector3.zero + Vector3.right * .5f;

        // STRUCTURE HOLDER
        targetStructureHolder.TryGet(out NetworkObject structureHolderX);
        GameObject structureHolder = structureHolderX.gameObject;
        structureHolder.transform.localPosition = Vector3.zero + Vector3.forward;

        // SPELL HOLDER
        targetSpellHolder.TryGet(out NetworkObject spellHolderX);
        GameObject spellHolder = spellHolderX.gameObject;
        spellHolder.transform.localPosition = Vector3.zero + Vector3.right * .5f;

        // POTION HOLDER
        targetPotionHolder.TryGet(out NetworkObject potionHolderX);
        GameObject potionHolder = potionHolderX.gameObject;
        potionHolder.transform.localPosition = Vector3.zero + Vector3.right * .5f;
    }

    // ----------------------------------------> UPDATE BUILD PLAYER <----------------------------------------
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerServerRpc()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Transform player = client.PlayerObject.transform;
            NetworkObject itemHolder = player.GetChild(1).GetChild(1).GetComponent<NetworkObject>();
            NetworkObject weaponHolder = player.GetChild(1).GetChild(1).GetChild(0).GetComponent<NetworkObject>();
            NetworkObject structureHolder = player.GetChild(1).GetChild(1).GetChild(1).GetComponent<NetworkObject>();
            NetworkObject spellHolder = player.GetChild(1).GetChild(1).GetChild(2).GetComponent<NetworkObject>();
            NetworkObject potionHolder = player.GetChild(1).GetChild(1).GetChild(3).GetComponent<NetworkObject>();
            UpdatePlayerClientRpc(itemHolder, weaponHolder, structureHolder, spellHolder, potionHolder);
        }
    }
    [ClientRpc]
    void UpdatePlayerClientRpc(NetworkObjectReference itemHolderRef, NetworkObjectReference weaponHolderRef, NetworkObjectReference structureHolderRef, NetworkObjectReference spellHolderRef, NetworkObjectReference potionHolderRef)
    {
        // TRY GET 
        itemHolderRef.TryGet(out NetworkObject itemHolder);
        weaponHolderRef.TryGet(out NetworkObject weaponHolder);
        structureHolderRef.TryGet(out NetworkObject structureHolder);
        spellHolderRef.TryGet(out NetworkObject spellHolder);
        potionHolderRef.TryGet(out NetworkObject potionHolder);

        // ASSIGN HOLDERS
        Holder holder = itemHolder.GetComponent<Holder>();
        holder.weaponHolderPart = weaponHolder.gameObject;
        holder.structureHolderPart = structureHolder.gameObject;
        holder.spellHolderPart = spellHolder.gameObject;
        holder.potionHolderPart = potionHolder.gameObject;
    }
}
