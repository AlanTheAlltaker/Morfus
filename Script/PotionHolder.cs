using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PotionHolder : NetworkBehaviour
{
    [SerializeField] GameObject invisiblePotion;
    public bool iP = false;
    [SerializeField] GameObject healingPotion;
    public bool hP = false;
    private void Update()
    {
        if (!IsOwner) { return; }
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
        if (iP)
        {
            invisiblePotion.SetActive(true);
        }
        if (hP)
        {
            healingPotion.SetActive(true);
        }
    }
}

