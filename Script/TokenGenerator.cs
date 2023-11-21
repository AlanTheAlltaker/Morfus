using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TokenGenerator : NetworkBehaviour
{
    [SerializeField] GameObject token;
    [SerializeField] AudioSource playAudio;
    [SerializeField] Transform tokenSpawnPoint;

    int nextUpdate = 1;

    [SerializeField] int cooldown = 10;
    // Update is called once per frame
    void Update()
    {

        if (!IsOwner) return;
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            cooldown -= 1;
            if (cooldown == 0)
            {
                ShootServerRpc();
                PlayAudioServerRpc();
                cooldown = 10;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc()
    {
        GameObject bullet = Instantiate(token, tokenSpawnPoint.position, tokenSpawnPoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
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
