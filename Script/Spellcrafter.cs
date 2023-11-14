using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Spellcrafter : NetworkBehaviour, IInteractable
{
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
        spellcrafterUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void CraftFlyingSpell()
    {
        spell = flyingSpell;
        StartCoroutine(Cooldown(10f));
    }
    public void CraftDetonationSpell()
    {
        spell = detionationSpell;
        StartCoroutine(Cooldown(10f));
    }
    public void Exit()
    {
        spellcrafterUI.SetActive(false);
        Cursor.lockState= CursorLockMode.Locked;
    }

    [ServerRpc (RequireOwnership = false)]
    void CraftSpellServerRpc()
    {
        GameObject bullet = Instantiate(spell, craftingSpot.position, craftingSpot.rotation);
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
