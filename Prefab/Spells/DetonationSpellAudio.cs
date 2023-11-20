using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DetonationSpellAudio : NetworkBehaviour
{
    [SerializeField] AudioSource explosionAudio;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        TurnOnAudioServerRpc();
        Invoke(nameof(DestroyServerRpc), 6.5f);

    }
    // PLAY AUDIO
    [ServerRpc(RequireOwnership = false)]
    void TurnOnAudioServerRpc()
    {
        TurnAudioOnClientRpc();

    }
    [ClientRpc]
    void TurnAudioOnClientRpc()
    {
        explosionAudio.Play();
    }
    // DESPAWN OBJECT
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
