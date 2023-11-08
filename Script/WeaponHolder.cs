using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{
    [SerializeField] GameObject gunCannon;
    [SerializeField] GameObject buldingGun;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) { return; }
        HideServerRpc(false, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideServerRpc(true, false);
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            HideServerRpc(false, true);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc(bool gunCannonB, bool buildingGunB)
    {
        HideClientRpc(gunCannonB, buildingGunB);
    }
    [ClientRpc]
    void HideClientRpc(bool gunCannonB, bool buildingGunB)
    {
        gunCannon.SetActive(gunCannonB);
        buldingGun.SetActive(buildingGunB);
    }
}
