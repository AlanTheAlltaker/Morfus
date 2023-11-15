using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    [SerializeField] GameObject flyingPotion;
    public bool fS = false;
    [SerializeField] GameObject detonationSpell;
    public bool dS = false;
    private void Update()
    {
        HideServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc()
    {
        HideClientRpc();
    }
    [ClientRpc]
    void HideClientRpc()
    {
        if (fS)
        {
            flyingPotion.SetActive(true);
        }
        if (dS)
        {
            detonationSpell.SetActive(true);
        }
    }
}

