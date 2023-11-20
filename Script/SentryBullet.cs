using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SentryBullet : NetworkBehaviour
{
    [SerializeField] float bulletSpeed = 10f;

    GameObject[] targets;
    GameObject nearestTarget;
    float distance;
    float nearestDistance = 10000;


    [SerializeField] float targetSpeed = 10f;
    [SerializeField] float startingCooldown = 2f;

    [SerializeField] bool isBlue;
    [SerializeField] bool isRed;


    void Awake()
    {
        Invoke(nameof(Destroy), 5f);
        if (isRed)
        {
            targets = GameObject.FindGameObjectsWithTag("PlayerBlue");
        }
        if (isBlue)
        {
            targets = GameObject.FindGameObjectsWithTag("PlayerRed");
        }

        for (int i = 0; i < targets.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, targets[i].transform.position);

            if(distance < nearestDistance)
            {
                nearestTarget = targets[i];
                nearestDistance = distance;
            }
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = this.transform.up * bulletSpeed;
    }

    private void Update()
    {
        if (!IsOwner) { return; }
        startingCooldown -= Time.deltaTime;
        if (startingCooldown <= 0)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = this.transform.up * -1f;
            transform.position = Vector3.MoveTowards(transform.position, nearestTarget.transform.position, targetSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
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