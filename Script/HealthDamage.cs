using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthDamage : NetworkBehaviour
{
    //Update timer
    int nextUpdate = 1;

    public NetworkVariable<int> healthPoint = new NetworkVariable<int>();

    [SerializeField] Transform redSpawner;
    [SerializeField] Transform blueSpawner;

    [SerializeField] int damage = 20;

    [SerializeField] bool isPlayer;
    [SerializeField] bool isStructure;
    [SerializeField] bool isBoss;

    public bool isHealing;
    float healingCooldown = 2f;

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
                Destroy(gameObject);
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
        if (collider.gameObject.GetComponent<Bullet>() || collider.gameObject.GetComponent<SentryBullet>())
        {
            if (IsServer)
            {
                GetComponent<HealthDamage>().healthPoint.Value -= damage;
                Debug.Log(GetComponent<HealthDamage>().healthPoint.Value);
                Destroy(collider.gameObject);
            }
        }
    }
    IEnumerator Cooldown()
    {
        TagsManager tagsManager = gameObject.GetComponent<TagsManager>();
        var script = gameObject.GetComponent<Movement>();
        script.enabled = false;
        yield return new WaitForSeconds(0.01f);
        if (tagsManager.tagsList.Contains("Blue"))
        {
            gameObject.transform.position = blueSpawner.position;
        }
        else if (tagsManager.tagsList.Contains("Red"))
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
}
