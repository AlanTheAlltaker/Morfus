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
        if (fP)
        {
            flyingPotion.SetActive(true);
        }
    }
}

