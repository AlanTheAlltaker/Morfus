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
    [SerializeField] AudioSource playAudio;


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
            PlayAudioServerRpc();
            tokens.Value += 1;
        }
    }
    [ServerRpc]
    void SetTokenValueServerRpc()
    {
        tokens.Value = 120;
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
