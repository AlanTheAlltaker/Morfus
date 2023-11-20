using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class HealthDamage : NetworkBehaviour
{
    //Update timer
    int nextUpdate = 1;

    public NetworkVariable<int> healthPoint = new NetworkVariable<int>();

    [SerializeField] Transform redSpawner;
    [SerializeField] Transform blueSpawner;

    [SerializeField] AudioSource takeDamageSound;

    [SerializeField] int damage = 20;

    public bool isPlayer;
    public bool isStructure;
    public bool isBoss;

    public bool isHealing;
    float healingCooldown = 2f;

    [SerializeField] TextMeshProUGUI textValue;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) { return; }
        healthPoint.Value = 100;
        if (isBoss)
        {
            healthPoint.Value = 1000;
        }

    }

    private void Update()
    {
        if (IsOwner && isPlayer)
        {
            if (!IsOwner) return;
            textValue.text = healthPoint.Value.ToString();
        }

        if (!IsServer) { return; }
        if (healthPoint.Value <= 0)
        {
            if (isPlayer)
            {
                healthPoint.Value = 100;
                TeleportPlayerClientRpc();
                Debug.Log("Witam");
            }
            if (isStructure || isBoss)
            {
                DestroyServerRpc();
            }
        }
        if (isHealing && !isBoss)
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                // Call your fonction
                healthPoint.Value += 10;
                Debug.Log(healthPoint.Value);
                healingCooldown -= 1f;
                Debug.Log(healingCooldown);
            }
            if (healingCooldown <= 0)
            {
                Debug.Log("Koniec");
                isHealing = false;
                healingCooldown = 2f;
            }
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<Bullet>())
        {
            if (IsServer)
            {
                GetComponent<HealthDamage>().healthPoint.Value -= damage;
                TurnOnAudioServerRpc();
                Destroy(collider.gameObject);
            }
        }
        if (collider.gameObject.GetComponent<SentryBullet>() && !isBoss)
        {
            if (IsServer)
            {
                GetComponent<HealthDamage>().healthPoint.Value -= damage;
                TurnOnAudioServerRpc();
                Destroy(collider.gameObject);
            }
        }
    }
    IEnumerator Cooldown()
    {
        var script = gameObject.GetComponent<Movement>();
        script.enabled = false;
        yield return new WaitForSeconds(0.01f);
        if (gameObject.tag == "PlayerBlue")
        {
            gameObject.transform.position = blueSpawner.position;
        }
        else if (gameObject.tag == "PlayerRed")
        {
            gameObject.transform.position = redSpawner.position;
        }
        Debug.Log("Odliczam");
        yield return new WaitForSeconds(0.01f);
        Debug.Log("Odpalono");
        script.enabled = true;
        Debug.Log("Your hp is " + healthPoint.Value);
    }
    [ClientRpc]
    void TeleportPlayerClientRpc()
    {
        StartCoroutine(Cooldown());
    }
    // DESPAWN OBJECT
    [ServerRpc(RequireOwnership = false)]
    void DestroyServerRpc()
    {
       gameObject.GetComponent<NetworkObject>().Despawn();
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
        takeDamageSound.Play();
    }
}
