using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Structurecrafter : NetworkBehaviour, IInteractable
{
    [SerializeField] Renderer objectRenderer;
    [SerializeField] Material red;
    [SerializeField] Material basic;
    [SerializeField] GameObject wallItem;
    [SerializeField] GameObject sentryItem;
    [SerializeField] GameObject tokenGeneratorItem;
    [SerializeField] GameObject StructurecrafterItem;
    [SerializeField] GameObject SpellcrafterItem;
    [SerializeField] GameObject PotioncrafterItem;
    [SerializeField] GameObject elevatorItem;

    [SerializeField] Transform craftingSpot;
    [SerializeField] GameObject spellcrafterUI;

    bool isCooldown = false;
    void Start()
    {
        spellcrafterUI.SetActive(false);
    }
    public void Interact()
    {
        if (isCooldown) return;
        spellcrafterUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    //EXIT
    public void Exit()
    {
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    //CHANGE COLOR
    [ClientRpc]
    void ChangeColorClientRpc()
    {
        if (!isCooldown)
        {
            isCooldown = true;
            objectRenderer.material = red;
        } else if (isCooldown) 
        {
            isCooldown = false;
            objectRenderer.material = basic;
        }
    }

    // CRAFT
    IEnumerator CraftItem(GameObject item, float cooldown)
    {
        if (!isCooldown)
        {
            ChangeColorClientRpc();
            yield return new WaitForSeconds(cooldown);
            Craft(item);
            ChangeColorClientRpc();
        }
    }
    void Craft(GameObject itemToCraft)
    {
        GameObject bullet = Instantiate(itemToCraft, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    // CRAFT WALL
    [ServerRpc(RequireOwnership = false)]
    public void CraftWallServerRpc()
    {
        StartCoroutine(CraftItem(wallItem, 2f));
    }

    // CRAFT SENTRY
    [ServerRpc(RequireOwnership = false)]
    public void CraftSentryServerRpc()
    {
        StartCoroutine(CraftItem(sentryItem, 5f));
    }

    // CRAFT TOKENGENERATOR
    [ServerRpc(RequireOwnership = false)]
    public void CraftTokenGeneratorServerRpc()
    {
        StartCoroutine(CraftItem(tokenGeneratorItem, 5f));
    }

    // CRAFT STRUCTURECRAFTER
    [ServerRpc(RequireOwnership = false)]
    public void CraftStrucutrecrafterServerRpc()
    {
        StartCoroutine(CraftItem(StructurecrafterItem, 10f));
    }

    // CRAFT SPELLCRAFTER
    [ServerRpc(RequireOwnership = false)]
    public void CRaftSpellcrafterServerRpc()
    {
        StartCoroutine(CraftItem(SpellcrafterItem, 10f));
    }

    // CRAFT POTIONCRAFTER
    [ServerRpc(RequireOwnership = false)]
    public void CraftPotioncrafterServerRpc()
    {
        StartCoroutine(CraftItem(PotioncrafterItem, 10f));
    }

    // CRAFT ELEVATOR
    [ServerRpc(RequireOwnership = false)]
    public void CraftElevatorServerRpc()
    {
        StartCoroutine(CraftItem(elevatorItem, 5f));
    }
    
}
