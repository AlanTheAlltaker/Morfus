using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TokenGenerator : NetworkBehaviour
{
    [SerializeField] GameObject token;
    [SerializeField] Transform tokenSpawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating(nameof(ShootServerRpc), 10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc()
    {
        GameObject bullet = Instantiate(token, tokenSpawnPoint.position, tokenSpawnPoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
