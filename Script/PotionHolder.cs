using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PotionHolder : MonoBehaviour
{
    [SerializeField] GameObject invisiblePotion;
    public bool iP = false;
    [SerializeField] GameObject healingPotion;
    public bool hP = false;
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

