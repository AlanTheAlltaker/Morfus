using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingBullet : NetworkBehaviour
{
    [SerializeField] float life = 2f;
    [SerializeField] float bulletSpeed = 2f;

    public GameObject buildingPrefab;
    [SerializeField] Transform player;

    bool dealtDamage = false;


    void Awake()
    {
        Invoke(nameof(Destroy), 3f);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        SpawnServerRpc(collision.GetContact(0).point + Vector3.up * 2.2f, player.rotation);
        Destroy();
    }
    public void Destroy()
    {
        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void SpawnServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject buildingBullet = Instantiate(buildingPrefab, position, rotation);
        buildingBullet.GetComponent<NetworkObject>().Spawn();
    }
}