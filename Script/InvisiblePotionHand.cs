using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InvisiblePotionHand : NetworkBehaviour
{
    [SerializeField] MeshRenderer playerRenderer;
    [SerializeField] MeshRenderer glassRenderer;
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(Potion());
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
}
