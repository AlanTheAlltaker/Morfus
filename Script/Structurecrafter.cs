using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Structurecrafter : NetworkBehaviour, IInteractable
{
    GameObject itemToCraft;
    [SerializeField] GameObject wallItem;
    [SerializeField] GameObject sentryItem;

    [SerializeField] Transform craftingSpot;
    [SerializeField] GameObject spellcrafterUI;

    void Start()
    {
        spellcrafterUI.SetActive(false);
    }
    public void Interact()
    {
        spellcrafterUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void CraftWall()
    {
        itemToCraft = wallItem;
        CraftSpellServerRpc();
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CraftSentry()
    {
        itemToCraft = sentryItem;
        CraftSpellServerRpc();
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Exit()
    {
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    [ServerRpc(RequireOwnership = false)]
    void CraftSpellServerRpc()
    {
        GameObject bullet = Instantiate(itemToCraft, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
