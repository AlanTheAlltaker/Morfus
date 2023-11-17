using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DetonationSpell : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float cooldown;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb.isKinematic = false;
        rb.velocity = transform.forward * 5f;
    }
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (cooldown <= 0)
        {
            if (collider.gameObject.GetComponent<HealthDamage>())
            {
                if (!IsServer) { return; }
                HealthDamage healthDamage = collider.gameObject.GetComponent<HealthDamage>();
                healthDamage.healthPoint.Value -= 100;
                Destroy(gameObject);
            }
        }
    }
}
