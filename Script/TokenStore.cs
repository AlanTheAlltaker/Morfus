using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class TokenStore : NetworkBehaviour
{
    public NetworkVariable<int> tokens = new NetworkVariable<int>();

    [SerializeField] TextMeshProUGUI textValue;

    private void Update()
    {
        if(!IsOwner) return;
        textValue.text = tokens.Value.ToString();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        SetTokenValueServerRpc();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Token>() != null)
        {
            if (!IsServer) { return; }
            tokens.Value += 1;
            Debug.Log("Posiadasz" + tokens.Value + " tokenów");
        }
    }
    [ServerRpc]
    void SetTokenValueServerRpc()
    {
        tokens.Value = 120;
    }
}
