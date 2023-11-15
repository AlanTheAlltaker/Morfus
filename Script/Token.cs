using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Token : NetworkBehaviour
{
    //[SerializeField] BuildingGun buildingGun;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HealthDamage>())
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
}
