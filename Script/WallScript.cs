using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WallScript : NetworkBehaviour
{
    NetworkVariable<float> wallHealth = new NetworkVariable<float>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        wallHealth.Value = 100f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            wallHealth.Value -= 20f;
            if (IsServer)
            {
                Destroy(collision.gameObject);
                if (wallHealth.Value <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
