using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReadyCube : NetworkBehaviour, IInteractable
{
    public void Interact()
    {
        ReadyPlayerServerRpc();
        Debug.Log("Akutalizacja");
    }

    // BUILD PLAYER
    [ServerRpc(RequireOwnership = false)]
    public void ReadyPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            Transform player = client.PlayerObject.transform;
            NetworkObject itemHolder = player.GetChild(1).GetChild(1).GetComponent<NetworkObject>();
            NetworkObject weaponHolder = player.GetChild(1).GetChild(1).GetChild(0).GetComponent<NetworkObject>();
            NetworkObject structureHolder = player.GetChild(1).GetChild(1).GetChild(1).GetComponent<NetworkObject>();
            NetworkObject spellHolder = player.GetChild(1).GetChild(1).GetChild(2).GetComponent<NetworkObject>();
            NetworkObject potionHolder = player.GetChild(1).GetChild(1).GetChild(3).GetComponent<NetworkObject>();
            ReadyPlayerClientRpc(itemHolder, weaponHolder, structureHolder, spellHolder, potionHolder);
        }
    }
    [ClientRpc]
    void ReadyPlayerClientRpc(NetworkObjectReference itemHolderRef, NetworkObjectReference weaponHolderRef, NetworkObjectReference structureHolderRef, NetworkObjectReference spellHolderRef, NetworkObjectReference potionHolderRef)
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

    [ClientRpc]
    void BuildPlayerClientRpc(
        NetworkObjectReference target3,
        NetworkObjectReference targetWeaponHolder,
        NetworkObjectReference targetStructureHolder,
        NetworkObjectReference targetSpellHolder,
        NetworkObjectReference targetPotionHolder)
    {
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

        // ASSIGN HOLDERS
        Holder holder = itemHolder.GetComponent<Holder>();
        holder.weaponHolderPart = weaponHolder;
        holder.structureHolderPart = structureHolder;
        holder.spellHolderPart = spellHolder;
        holder.potionHolderPart = potionHolder;
    }
}
