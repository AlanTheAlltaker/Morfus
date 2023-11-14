using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Potioncrafter : NetworkBehaviour, IInteractable
{
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
    public void CraftInvisiblePotion()
    {
        potion = invisiblePotion;
        StartCoroutine(Cooldown(10f));
    }
    public void CraftHealingPotion()
    {
        potion = healingPotion;
        StartCoroutine(Cooldown(10f));
    }
    public void Exit()
    {
        craftingUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    [ServerRpc(RequireOwnership = false)]
    void CraftSpellServerRpc()
    {
        GameObject bullet = Instantiate(potion, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
    IEnumerator Cooldown(float cooldown)
    {
        if (!isCooldown)
        {
            isCooldown = true;
            craftingUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            yield return new WaitForSeconds(cooldown);
            CraftSpellServerRpc();
            isCooldown = false;
        }
    }
}
