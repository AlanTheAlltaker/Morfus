using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandItem : NetworkBehaviour
{
    StructureHolder structureHolder;

    [SerializeField] bool wallItem;
    [SerializeField] bool sentryItem;
    [SerializeField] bool elevatorItem;
    [SerializeField] bool tokenGeneratorItem;
    [SerializeField] bool Spellcrafter;
    [SerializeField] bool Structurecrafter;
    [SerializeField] bool Potioncrafter;
    [SerializeField] bool CannonWarMachine;
    [SerializeField] bool TeleportItem;
    [SerializeField] bool HealingStationItem;
    [SerializeField] bool TokenCollector;

    [SerializeField] AudioSource playAudio;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayAudioServerRpc();
        if (collision.gameObject.name == "Player(Clone)")
        {
            GameObject pH = collision.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
            structureHolder = pH.GetComponent<StructureHolder>();
            if (!structureHolder.wI && !structureHolder.sI && !structureHolder.eI && !structureHolder.tGI && !structureHolder.stCI && !structureHolder.spCI && !structureHolder.poCI && !structureHolder.cWMI && !structureHolder.tI && !structureHolder.hSI && !structureHolder.tCI)
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
        GameObject sH = playerTransform.GetChild(1).GetChild(2).GetChild(1).gameObject;
        structureHolder = sH.GetComponent<StructureHolder>();
        if (wallItem)
        {
            structureHolder.wI = true;
        }
        if (sentryItem)
        {
            structureHolder.sI = true;
        }
        if (elevatorItem)
        {
            structureHolder.eI = true;
        }
        if (tokenGeneratorItem)
        {
            structureHolder.tGI = true;
        }
        if (Structurecrafter)
        {
            structureHolder.stCI = true;
        }
        if (Potioncrafter)
        {
            structureHolder.poCI = true;
        }
        if (Spellcrafter)
        {
            structureHolder.spCI = true;
        }
        if (CannonWarMachine)
        {
            structureHolder.cWMI = true;
        }
        if (TeleportItem)
        {
            structureHolder.tI = true;
        }
        if (HealingStationItem)
        {
            structureHolder.hSI = true;
        }
        if (TokenCollector)
        {
            structureHolder.tCI = true;
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
