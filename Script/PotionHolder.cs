using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PotionHolder : MonoBehaviour
{
    [SerializeField] GameObject flyingPotion;
    public bool fP = false;
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
        if (fP)
        {
            flyingPotion.SetActive(gunCannonB);
        }
    }
}

