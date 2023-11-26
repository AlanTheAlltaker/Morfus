using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TokenPlant : NetworkBehaviour, IInteractable
{
    public NetworkVariable<int> tokensCollected = new NetworkVariable<int>();
    [SerializeField] TextMeshPro collectedNumber;
    
    float cooldown = 10f;

    public void Interact()
    {
        CollectServerRpc();
    }
    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            if (IsServer)
            {
                tokensCollected.Value += 1;
            }
            collectedNumber.text = tokensCollected.Value.ToString();
            cooldown = 10f;
        }
        
    }
    [ServerRpc (RequireOwnership = false)]
    void CollectServerRpc(ServerRpcParams serverRpcParams = default)
    {

        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            Transform player = client.PlayerObject.transform;
            TokenStorage tokenStorage = player.GetComponent<TokenStorage>();
            tokenStorage.tokens.Value += tokensCollected.Value;
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
