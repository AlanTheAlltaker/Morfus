using UnityEngine;
using Unity.Netcode;
using TMPro;

public class TokenStorage : NetworkBehaviour
{
    public NetworkVariable<int> tokens = new NetworkVariable<int>();

    [SerializeField] TextMeshProUGUI textValue;

    private void Update()
    {
        if (!IsOwner) return;
        textValue.text = tokens.Value.ToString();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        SetTokenValueServerRpc();
    }
    [ServerRpc]
    void SetTokenValueServerRpc()
    {
        tokens.Value = 100;
    }
}
