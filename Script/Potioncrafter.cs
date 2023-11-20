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

    [SerializeField] AudioSource playAudio;


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
    IEnumerator CraftItem(GameObject item, float cooldown, TokenStore tokenStore, int cost)
    {
        if (!isCooldown && tokenStore.tokens.Value >= cost)
        {
            tokenStore.tokens.Value -= cost;
            ChangeColorClientRpc();
            yield return new WaitForSeconds(cooldown);
            PlayAudioServerRpc();
            Craft(item);
            ChangeColorClientRpc();
        }
        else if (!isCooldown && tokenStore.tokens.Value < cost)
        {
            Debug.Log("U dont have enough tokens");
        }
    }
    void Craft(GameObject itemToCraft)
    {
        GameObject bullet = Instantiate(itemToCraft, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    // CRAFT INVISIBLE POTION
    public void ButtonCraftInvisiblePotion()
    {
        CraftInvisiblePotionServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftInvisiblePotionServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(invisiblePotion, 15f, tokenStore, 10));
        }
    }
    // CRAFT HEALING POTION
    public void ButtonCraftHealingPotion()
    {
        CraftHealingPotionServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftHealingPotionServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(healingPotion, 15f, tokenStore, 10));
        }
    }
    // PLAY AUDIO
    [ServerRpc(RequireOwnership = false)]
    void PlayAudioServerRpc()
    {
        PlayAudioClientRpc();
    }
    [ClientRpc]
    void PlayAudioClientRpc()
    {
        playAudio.Play();
    }
}
