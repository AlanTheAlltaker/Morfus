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
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 2;
    }

    private void OnTriggerEnter(Collider collision)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        if (collision.gameObject.name == "Player(Clone)")
        {
            GameObject pH = collision.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
            structureHolder = pH.GetComponent<StructureHolder>();
            if (!structureHolder.wI && !structureHolder.sI && !structureHolder.eI && !structureHolder.tGI)
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
    }
}
