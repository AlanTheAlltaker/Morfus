using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Structurecrafter : NetworkBehaviour, IInteractable
{
    bool isCooldown = false;

    [SerializeField] Renderer objectRenderer;
    [SerializeField] Material red;
    [SerializeField] Material basic;

    [SerializeField] Transform craftingSpot;

    [SerializeField] AudioSource playAudio;
    [SerializeField] AudioSource workingCrafter;

    [SerializeField] GameObject spellcrafterUI;


    [SerializeField] GameObject wallItem;
    // Start is called before the first frame update
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
    // TURN ON AUDIO
    [ServerRpc(RequireOwnership = false)]
    void TurnOnAudioServerRpc(bool iswhat)
    {
        TurnAudioOnClientRpc(iswhat);

    }
    [ClientRpc]
    void TurnAudioOnClientRpc(bool iswhat)
    {
        workingCrafter.enabled = iswhat;
    }

    //CHANGE COLOR
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
    IEnumerator CraftItem(GameObject item, float cooldown, TokenStorage tokenStorage, int cost)
    {
        if (!isCooldown && tokenStorage.tokens.Value >= cost)
        {
            tokenStorage.tokens.Value -= cost;
            ChangeColorClientRpc();
            TurnOnAudioServerRpc(true);
            yield return new WaitForSeconds(cooldown);
            TurnOnAudioServerRpc(false);
            PlayAudioServerRpc();
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
    public void ButtonCraftWall()
    {
        Exit();
        CraftWallServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftWallServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStorage tokenStore = player.GetComponent<TokenStorage>();
            StartCoroutine(CraftItem(wallItem, 1f, tokenStore, 1));
        }
    }
}
