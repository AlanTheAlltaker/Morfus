using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Wall : NetworkBehaviour, IInteractable
{
    [SerializeField] BoxCollider wall;
    [SerializeField] BoxCollider wallFront;
    [SerializeField] BoxCollider wallBack;
    [SerializeField] BoxCollider wallLeft;
    [SerializeField] BoxCollider wallRight;
    [SerializeField] BoxCollider wallUp;
    [SerializeField] BoxCollider wallDown;

    [SerializeField] Material wallMaterial;
    [SerializeField] Material ghostMaterial;

    [SerializeField] Renderer wallRenderer;


    public void Interact()
    {
        Debug.Log("Open");
        StartCoroutine(TurnOn());
    }
    IEnumerator TurnOn()
    {
        wallRenderer.material = ghostMaterial;
        wall.enabled = false;
        wallFront.enabled = false;
        wallBack.enabled = false;
        wallLeft.enabled = false;
        wallRight.enabled = false;
        wallUp.enabled = false;
        wallDown.enabled = false;
        yield return new WaitForSeconds(2f);
        wallRenderer.material = wallMaterial;
        wall.enabled = true;
        wallFront.enabled = true;
        wallBack.enabled = true;
        wallLeft.enabled = true;
        wallRight.enabled = true;
        wallUp.enabled = true;
        wallDown.enabled = true;
    }
}
