using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{
    [SerializeField] GameObject gunCannon;
    [SerializeField] GameObject structureHolder;
    [SerializeField] GameObject spellHolder;
    [SerializeField] GameObject emptyHand;
    [SerializeField] GameObject potionHolder;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; }
        HideServerRpc(false, false, false, false, false);
    }
    void Update()
    {
        if (!IsOwner) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideServerRpc(true, false, false, false, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HideServerRpc(false, false, true, false, false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HideServerRpc(false, true, false, false, false);
        }
        if (Input.GetMouseButtonDown(2))
        {
            HideServerRpc(false, false, false, true, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HideServerRpc(false, false, false, false, true);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc(bool gunCannonB, bool buildingGunB, bool spellHolderB, bool emptyHandB, bool potionHolderB)
    {
        HideClientRpc(gunCannonB, buildingGunB, spellHolderB, emptyHandB, potionHolderB);
    }
    [ClientRpc]
    void HideClientRpc(bool gunCannonB, bool buildingGunB, bool spellHolderB, bool emptyHandB, bool potionHolderB)
    {
        gunCannon.SetActive(gunCannonB);
        structureHolder.SetActive(buildingGunB);
        spellHolder.SetActive(spellHolderB);
        emptyHand.SetActive(emptyHandB);
        potionHolder.SetActive(potionHolderB);
    }
}
