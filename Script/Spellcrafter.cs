using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spellcrafter : NetworkBehaviour, IInteractable
{
    [SerializeField] GameObject spell;
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
    public void CraftSpell()
    {
        CraftSpellServerRpc();
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    [ServerRpc (RequireOwnership = false)]
    void CraftSpellServerRpc()
    {
        GameObject bullet = Instantiate(spell, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
