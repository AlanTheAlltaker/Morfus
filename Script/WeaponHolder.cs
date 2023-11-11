using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{
    [SerializeField] GameObject gunCannon;
    [SerializeField] GameObject structureHolder;
    [SerializeField] GameObject potionHolder;
    [SerializeField] GameObject emptyHand;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; }
        HideServerRpc(false, false, false, true);
    }

    // Update is called once per frame
    void Update()
    {
        //Position
        //transform.localRotation = player.localRotation;



        //Not movement
        if (!IsOwner) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideServerRpc(true, false, false, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HideServerRpc(false, false, true, false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HideServerRpc(false, true, false, false);
        }
        if (Input.GetMouseButtonDown(2))
        {
            HideServerRpc(false, false, false, true);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc(bool gunCannonB, bool buildingGunB, bool potionHolderB, bool emptyHandB)
    {
        HideClientRpc(gunCannonB, buildingGunB, potionHolderB, emptyHandB);
    }
    [ClientRpc]
    void HideClientRpc(bool gunCannonB, bool buildingGunB, bool potionHolderB, bool emptyHandB)
    {
        gunCannon.SetActive(gunCannonB);
        structureHolder.SetActive(buildingGunB);
        potionHolder.SetActive(potionHolderB);
        emptyHand.SetActive(emptyHandB);
    }
}
