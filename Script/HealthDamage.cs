using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthDamage : NetworkBehaviour
{
    public NetworkVariable<int> healthPoint = new NetworkVariable<int>();

    [SerializeField] Transform redSpawner;
    [SerializeField] Transform blueSpawner;

    [SerializeField] int damage = 20;

    [SerializeField] bool isPlayer;
    [SerializeField] bool isStructure;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        healthPoint.Value = 100;
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
            if (isStructure)
            {
                Destroy(gameObject);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Token>() != null) 
        {
            if (!IsServer) { return; }
            //buildingGun.tokens.Value += 1;
            //Debug.Log("Posiadasz" + buildingGun.tokens.Value + " tokenów");
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
