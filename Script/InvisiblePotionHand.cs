using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InvisiblePotionHand : NetworkBehaviour
{
    [SerializeField] MeshRenderer playerRenderer;
    [SerializeField] MeshRenderer glassRenderer;

    [SerializeField] PotionHolder potionHolder;
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(Potion());
            FPServerRpc();
            HideServerRpc();
        }
    }
    IEnumerator Potion()
    {
        playerRenderer.enabled = false;
        glassRenderer.enabled = false;
        yield return new WaitForSeconds(10f);
        playerRenderer.enabled = true;
        glassRenderer.enabled = true;
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
