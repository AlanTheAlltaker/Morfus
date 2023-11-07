using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthDamage : NetworkBehaviour
{
    public NetworkVariable<int> healthPoint = new NetworkVariable<int>();

    [SerializeField] BuildingGun buildingGun;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        healthPoint.Value = 100;
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<Bullet>() || collider.gameObject.GetComponent<SentryBullet>())
        {
            if (IsServer)
            {
                GetComponent<HealthDamage>().healthPoint.Value -= 0;
                Debug.Log(GetComponent<HealthDamage>().healthPoint.Value);
                Destroy(collider.gameObject);
                if (healthPoint.Value <= 0)
                {
                    healthPoint.Value = 100;
                    TeleportPlayerClientRpc();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Token>() != null) 
        {
            if (!IsServer) { return; }
            buildingGun.tokens.Value += 1;
            Debug.Log("Posiadasz" + buildingGun.tokens.Value + " tokenów");
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
            gameObject.transform.position = new Vector3(-100f, 3f, 100f);
        }
        else if (tagsManager.tagsList.Contains("Red"))
        {
            gameObject.transform.position = new Vector3(-100f, 3f, -100f);
        }
        Debug.Log("Odliczam");
        yield return new WaitForSeconds(2f);
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
