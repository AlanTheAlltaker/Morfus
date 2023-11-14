using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Structurecrafter : NetworkBehaviour, IInteractable
{
    GameObject itemToCraft;
    [SerializeField] GameObject wallItem;
    [SerializeField] GameObject sentryItem;
    [SerializeField] GameObject tokenGeneratorItem;
    [SerializeField] GameObject StructurecrafterItem;
    [SerializeField] GameObject SpellcrafterItem;
    [SerializeField] GameObject PotioncrafterItem;

    [SerializeField] Transform craftingSpot;
    [SerializeField] GameObject spellcrafterUI;

    bool isCooldown = false;
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
        
        StartCoroutine(Cooldown(10f));
    }
    public void CraftSentry()
    {
        itemToCraft = sentryItem;
        StartCoroutine(Cooldown(10f));
    }
    public void CraftTokenGenerator()
    {
        itemToCraft = tokenGeneratorItem;
        StartCoroutine(Cooldown(10f));
    }
    public void CraftSpellcrafter()
    {
        itemToCraft = SpellcrafterItem;
        StartCoroutine(Cooldown(10f));
    }
    public void CraftStructurecrafter()
    {
        itemToCraft = StructurecrafterItem;
        StartCoroutine(Cooldown(10f));
    }
    public void CraftPotioncrafter()
    {
        itemToCraft = PotioncrafterItem;
        StartCoroutine(Cooldown(10f));
    }
    public void Exit()
    {
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    [ServerRpc(RequireOwnership = false)]
    void CraftSpellServerRpc()
    {
        itemToCraft = wallItem;
        GameObject bullet = Instantiate(itemToCraft, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
    IEnumerator Cooldown(float cooldown)
    {
        if (!isCooldown)
        {
            isCooldown = true;
            spellcrafterUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            yield return new WaitForSeconds(cooldown);
            CraftSpellServerRpc();
            isCooldown = false;
        }
    }
}
