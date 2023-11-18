using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DetonationSpellHand : NetworkBehaviour
{
    [SerializeField] GameObject detonationSpell;
    [SerializeField] Transform droper;
    [SerializeField] SpellHolder spellHolder;
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0))
        {
            SpawnServerRpc();
            FPServerRpc();
            HideServerRpc();
        }
    }

    //Spawn detonation spell
    [ServerRpc(RequireOwnership = false)]
    void SpawnServerRpc()
    {
        GameObject bullet = Instantiate(detonationSpell, droper.position, droper.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
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

    //Set false
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        spellHolder.dS = false;
    }
}
