using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TokenStore : NetworkBehaviour
{
    public NetworkVariable<int> tokens = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        tokens.Value = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Token>() != null)
        {
            if (!IsServer) { return; }
            tokens.Value += 1;
            Debug.Log("Posiadasz" + tokens.Value + " token�w");
        }
    }
}
