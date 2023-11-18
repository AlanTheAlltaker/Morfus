using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class DropHand : NetworkBehaviour
{
    [SerializeField] GameObject itemToDrop;
    [SerializeField] Transform droper;
    [SerializeField] SpellHolder spellHolder;

    [SerializeField] bool isDetonationSpell;
    [SerializeField] bool isFlyingSpell;
    private void OnEnable()
    {
        if (isDetonationSpell)
        {
            if (!spellHolder.dS)
            {
                HideServerRpc();
            }
        }
        if (isFlyingSpell)
        {
            if (!spellHolder.fS)
            {
                HideServerRpc();
            }
        }
    }
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShootServerRpc(droper.position, gameObject.transform.rotation);
            FPServerRpc();
            HideServerRpc();
        }
    }

    //Drop item
    [ServerRpc (RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(itemToDrop, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
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
        if (isDetonationSpell)
        {
            spellHolder.dS = false;
        }
        if (isFlyingSpell)
        {
            spellHolder.fS = false;
        }
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
