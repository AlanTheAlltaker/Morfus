using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlyingSpell : NetworkBehaviour
{
    [SerializeField] PlayerEffect playerEffect;
    [SerializeField] Movement movement;
    [SerializeField] SpellHolder spellHolder;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0) && !movement.isGrounded)
        {
            SetInvisibleServerRpc();
            FPServerRpc();
            HideServerRpc();
        }
    }
    // Set flying effect true
    [ServerRpc(RequireOwnership = false)]
    void SetInvisibleServerRpc()
    {
        SetInvisibleClientRpc();
    }
    [ClientRpc]
    void SetInvisibleClientRpc()
    {
        playerEffect.flyingEffect = true;
    }
    // HIDE
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
    // SET FALSE
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        spellHolder.fS = false;
    }
}
