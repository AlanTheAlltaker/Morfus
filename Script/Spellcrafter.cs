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
    [SerializeField] GameObject detonationSpell;
    GameObject spell;
    [SerializeField] Transform craftingSpot;
    [SerializeField] GameObject spellcrafterUI;

    [SerializeField] AudioSource playAudio;


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
        }else if (!isCooldown && tokenStore.tokens.Value < cost)
        {
            Debug.Log("U dont have enough tokens");
        }
    }
    void Craft(GameObject itemToCraft)
    {
        GameObject bullet = Instantiate(itemToCraft, craftingSpot.position, craftingSpot.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    // CRAFT FLYING SPELL
    public void ButtonCraftFlyingSpell()
    {
        CraftFlyingSpellServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftFlyingSpellServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(flyingSpell, 15f, tokenStore, 10));
        }
    }

    // CRAFT DETONATION SPELL
    public void ButtonCraftDetonationSpell()
    {
        CraftDetonationSpellServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftDetonationSpellServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(detonationSpell, 30f, tokenStore, 15));
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
