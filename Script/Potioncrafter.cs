using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Potioncrafter : NetworkBehaviour, IInteractable
{
    [SerializeField] Renderer objectRenderer;
    [SerializeField] Material red;
    [SerializeField] Material basic;

    [SerializeField] GameObject invisiblePotion;
    [SerializeField] GameObject healingPotion;
    GameObject potion;
    [SerializeField] Transform craftingSpot;
    [SerializeField] GameObject craftingUI;

    bool isCooldown = false;

    void Start()
    {
        craftingUI.SetActive(false);
    }
    public void Interact()
    {
        craftingUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    // EXIT
    public void Exit()
    {
        craftingUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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

    // CRAFT INVISIBLE POTION
    [ServerRpc(RequireOwnership = false)]
    public void CraftInvisiblePotionServerRpc()
    {
        StartCoroutine(CraftItem(invisiblePotion, 15f));
    }
    // CRAFT HEALING POTION
    [ServerRpc(RequireOwnership = false)]
    public void CraftHealingPotionServerRpc()
    {
        StartCoroutine(CraftItem(healingPotion, 15f));
    }
}
