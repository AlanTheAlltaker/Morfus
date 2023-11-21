using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellItem : NetworkBehaviour
{
    [SerializeField] bool isFlyingSpellItem;
    [SerializeField] bool isDetonationSpellItem;

    [SerializeField] AudioSource playAudio;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 2;
    }
    SpellHolder spellHolder;
    private void OnCollisionEnter(Collision collision)
    {
        PlayAudioServerRpc();
        if (collision.gameObject.name == "Player(Clone)")
        {
            GameObject pH = collision.transform.GetChild(1).GetChild(2).GetChild(2).gameObject;
            spellHolder = pH.GetComponent<SpellHolder>();
            
            if (!spellHolder.dS && !spellHolder.fS)
            {
                FPServerRpc(collision.gameObject.GetComponent<NetworkObject>());
                DestroyServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
        DestroyClientRpc();
    }
    [ClientRpc]
    void DestroyClientRpc()
    {
        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc(NetworkObjectReference target)
    {
        FPClientRpc(target);
    }
    [ClientRpc]
    void FPClientRpc(NetworkObjectReference target)
    {
        target.TryGet(out NetworkObject player);
        Transform playerTransform = player.GetComponent<Transform>();
        GameObject pH = playerTransform.GetChild(1).GetChild(2).GetChild(2).gameObject;
        spellHolder = pH.GetComponent<SpellHolder>();
        if (isDetonationSpellItem)
        {
            spellHolder.dS = true;
        }
        if (isFlyingSpellItem)
        {
            spellHolder.fS = true;
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
