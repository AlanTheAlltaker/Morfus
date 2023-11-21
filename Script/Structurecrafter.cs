using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Structurecrafter : NetworkBehaviour, IInteractable
{
    [SerializeField] Renderer objectRenderer;
    [SerializeField] Material red;
    [SerializeField] Material basic;
    [SerializeField] GameObject wallItem;
    [SerializeField] GameObject sentryItem;
    [SerializeField] GameObject tokenGeneratorItem;
    [SerializeField] GameObject StructurecrafterItem;
    [SerializeField] GameObject SpellcrafterItem;
    [SerializeField] GameObject PotioncrafterItem;
    [SerializeField] GameObject elevatorItem;
    [SerializeField] GameObject CannonWarMachineItem;
    [SerializeField] GameObject TeleportItem;
    [SerializeField] GameObject HealingStationItem;
    [SerializeField] GameObject TokenCollectorItem;

    [SerializeField] Transform craftingSpot;
    [SerializeField] GameObject spellcrafterUI;

    [SerializeField] AudioSource playAudio;

    [SerializeField] AudioSource workingCrafter;


    bool isCooldown = false;
    void Start()
    {
        spellcrafterUI.SetActive(false);
        workingCrafter.enabled = false;
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

    //CHANGE COLOR
    [ClientRpc]
    void ChangeColorClientRpc()
    {
        if (!isCooldown)
        {
            isCooldown = true;
            objectRenderer.material = red;
        } else if (isCooldown) 
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
            TurnOnAudioServerRpc(true);
            yield return new WaitForSeconds(cooldown);
            TurnOnAudioServerRpc(false);
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

    // CRAFT WALL
    public void ButtonCraftWall()
    {
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
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(wallItem, 1f, tokenStore, 1));
        }
    }

    // CRAFT SENTRY
    public void ButtonCraftSentry()
    {
        CraftSentryServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftSentryServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(sentryItem, 5f, tokenStore, 5));
        }
    }

    // CRAFT TOKENGENERATOR
    public void ButtonCraftTokenGenerator()
    {
        CraftTokenGeneratorServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftTokenGeneratorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(tokenGeneratorItem, 5f, tokenStore, 5));
        }
    }

    // CRAFT STRUCTURECRAFTER
    public void ButtonCrafStrucutrecrafter()
    {
        CraftStructurecrafterServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftStructurecrafterServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(StructurecrafterItem, 15f, tokenStore, 15));
        }
    }

    // CRAFT POTIONCRAFTER
    public void ButtonCraftPotioncrafter()
    {
        CraftPotioncrafterServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftPotioncrafterServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(PotioncrafterItem, 15f, tokenStore, 15));
        }
    }

    // CRAFT SPELLCRAFTER
    public void ButtonCraftSpellcrafter()
    {
        CraftSpellcrafterServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftSpellcrafterServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(SpellcrafterItem, 15f, tokenStore, 15));
        }
    }

    // CRAFT ELEVATOR
    public void ButtonCraftElevator()
    {
        CraftElevatorServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftElevatorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(elevatorItem, 2f, tokenStore, 5));
        }
    }

    // CRAFT CANNON WAR MACHINE
    public void ButtonCraftCannonWarMachine()
    {
        CraftCannonWarMachineServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftCannonWarMachineServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(CannonWarMachineItem, 60f, tokenStore, 60));
        }
    }
    // CRAFT CANNON WAR MACHINE
    public void ButtonCraftTeleport()
    {
        CraftTeleportServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftTeleportServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(TeleportItem, 30f, tokenStore, 15));
        }
    }
    // CRAFT HEALING STATION
    public void ButtonCraftHealingStation()
    {
        CraftHealingStationServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CraftHealingStationServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(HealingStationItem, 15f, tokenStore, 15));
        }
    }
    // CRAFT TOKEN COLLECTOR
    public void ButtonCraftTokenCollector()
    {
        TokenCollectorServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TokenCollectorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            TokenStore tokenStore = player.GetComponent<TokenStore>();
            StartCoroutine(CraftItem(TokenCollectorItem, 10f, tokenStore, 10));
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
    // PLAY FOOTSTEPS
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
}
