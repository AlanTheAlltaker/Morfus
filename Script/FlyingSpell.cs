using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlyingSpell : NetworkBehaviour
{
    [SerializeField] Flying fly;
    [SerializeField] Movement movement;
    [SerializeField] SpellHolder spellHolder;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0) && !movement.isGrounded)
        {
            FPServerRpc();
            HideServerRpc();
        }
    }
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
