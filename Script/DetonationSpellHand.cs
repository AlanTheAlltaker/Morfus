using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DetonationSpellHand : MonoBehaviour
{
    [SerializeField] GameObject detonationSpell;
    [SerializeField] Transform droper;
    [SerializeField] SpellHolder spellHolder;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SpawnServerRpc(droper.position, droper.rotation);
            spellHolder.dS = false;
            HideServerRpc();
        }
    }

    //Spawn detonation spell
    [ServerRpc(RequireOwnership = false)]
    void SpawnServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(detonationSpell, position, rotation);
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
}
