using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class HealingPotionExplosion : NetworkBehaviour
{
    HealthDamage healthDamage;
    bool isCooldown;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Invoke(nameof(Destroy), 0.5f);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<HealthDamage>())
        {
            healthDamage = collision.gameObject.GetComponent<HealthDamage>();
            if (healthDamage.healthPoint.Value <= 100 && !healthDamage.isBoss) 
            {
                healthDamage.isHealing = true;
            }
        }
    }
    void Destroy()
    {
        if (!IsServer) return;
        Destroy(gameObject);
    }
}
