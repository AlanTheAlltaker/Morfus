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
    public void Exit()
    {
        spellcrafterUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    //CRAFT WALL
    [ServerRpc(RequireOwnership = false)]
    public void CraftWallServerRpc()
    {
        StartCoroutine(CraftItem(wallItem, 10f));
    }
    IEnumerator CraftItem(GameObject item, float cooldown)
    {
        if (!isCooldown)
        {
            isCooldown = true;
            spellcrafterUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            yield return new WaitForSeconds(cooldown);
            Craft(item);
            isCooldown = false;
        }
    }
    void Craft(GameObject itemToCraft)
    {
        GameObject bullet = Instantiate(itemToCraft, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
