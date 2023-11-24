using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

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
        NetworkObjectReference target, 
        NetworkObjectReference target2,
        NetworkObjectReference target3,
        NetworkObjectReference targetWeaponHolder, 
        NetworkObjectReference targetStructureHolder,
        NetworkObjectReference targetSpellHolder,
        NetworkObjectReference targetPotionHolder)
    {
        target.TryGet(out NetworkObject playerX);
        Transform player = playerX.transform;

        // CAMERA  HOLDER
        target2.TryGet(out NetworkObject cameraHolderX);
        GameObject cameraHolder = cameraHolderX.gameObject;
        cameraHolder.GetComponent<MouseRotation>().playerBody = player;
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
}
