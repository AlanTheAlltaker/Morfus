using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] float bouncingHeight = 2f;

    [SerializeField] AudioSource playAudio;

    void Awake()
    {
        Invoke(nameof(Destroy), 3f);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;   
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed + this.transform.up * bouncingHeight;
        TurnOnAudioServerRpc();

        if (collision.gameObject.GetComponent<SentryBullet>())
        {
            Destroy();
        }
    }
    public void Destroy()
    {
        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
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
        playAudio.Play();
    }
}