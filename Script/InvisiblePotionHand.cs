using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InvisiblePotionHand : NetworkBehaviour
{
    [SerializeField] PotionHolder potionHolder;
    [SerializeField] PlayerEffect playerEffect;
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButton(0) && potionHolder.iP)
        {
            SetInvisibleServerRpc();
            FPServerRpc();
            HideServerRpc();
        }
    }
    //Set invisible effect true
    [ServerRpc (RequireOwnership = false)]
    void SetInvisibleServerRpc()
    {
        SetInvisibleClientRpc();
    }
    [ClientRpc]
    void SetInvisibleClientRpc()
    {
        playerEffect.invisibleEffect = true;
    }
    //Set false
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        potionHolder.iP = false;
    }

    //Hide
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc()
    {
        HideClientRpc();
    }
    [ClientRpc]
    void HideClientRpc()
    {
        gameObject.SetActive(false);
    }
}
