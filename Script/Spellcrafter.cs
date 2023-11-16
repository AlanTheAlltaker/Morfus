using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Spellcrafter : NetworkBehaviour, IInteractable
{
    [SerializeField] Renderer objectRenderer;
    [SerializeField] Material red;
    [SerializeField] Material basic;

    [SerializeField] GameObject flyingSpell;
    [SerializeField] GameObject detionationSpell;
    GameObject spell;
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

    // EXIT
    public void Exit()
    {
        spellcrafterUI.SetActive(false);
        Cursor.lockState= CursorLockMode.Locked;
    }

    // CHANGE COLOR
    [ClientRpc]
    void ChangeColorClientRpc()
    {
        if (!isCooldown)
        {
            isCooldown = true;
            objectRenderer.material = red;
        }
        else if (isCooldown)
        {
            isCooldown = false;
            objectRenderer.material = basic;
        }
    }

    // CRAFT FUNCTION
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

    // CRAFT FLYING SPELL
    [ServerRpc(RequireOwnership = false)]
    public void CraftFlyingSpellServerRpc()
    {
        StartCoroutine(CraftItem(flyingSpell, 15f));
    }

    // CRAFT DETONATION SPELL
    [ServerRpc(RequireOwnership = false)]
    public void CraftDetonationSpellServerRpc()
    {
        StartCoroutine(CraftItem(detionationSpell, 30f));
    }
}
