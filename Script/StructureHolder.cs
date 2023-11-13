using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StructureHolder : NetworkBehaviour
{
    [SerializeField] GameObject wallItem;
    public bool wI = false;
    [SerializeField] GameObject sentryItem;
    public bool sI = false;
    [SerializeField] GameObject elevatorItem;
    public bool eI = false;
    [SerializeField] GameObject tokenGeneratorItem;
    public bool tGI = false;

    private void Update()
    {
        HideServerRpc(true);
    }
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc(bool gunCannonB)
    {
        HideClientRpc(gunCannonB);
    }
    [ClientRpc]
    void HideClientRpc(bool gunCannonB)
    {
        if (wI)
        {
            wallItem.SetActive(gunCannonB);
        }
        if (sI)
        {
            sentryItem.SetActive(gunCannonB);
        }
        if (eI)
        {
            elevatorItem.SetActive(gunCannonB);
        }
        if (tGI)
        {
            tokenGeneratorItem.SetActive(gunCannonB);
        }
    }
}
