using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingRay : NetworkBehaviour
{
    Vector3 place;
    RaycastHit hit;

    [SerializeField] GameObject structureGhost;
    [SerializeField] GameObject strucutre;
    GameObject tempGhost;
    public Transform InteractorSource;


    bool ghostObjectSpawned = false;

    GameObject[] targets;
    GameObject nearestTarget;
    float distance;
    float nearestDistance = 10000;

    [SerializeField] float gridDistance;
    [SerializeField] float distanceFromGround;
    [SerializeField] bool wallItem;
    private void OnEnable()
    {
        if (!IsOwner) return;
        nearestDistance = 10000;
        ghostObjectSpawned = false;
    }
    private void OnDisable()
    {
        if (!IsOwner) { return; }
        Destroy(tempGhost);
    }

    void Update()
    {
        if (!IsOwner) return;
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        if (Physics.Raycast(r, out hit, hit.distance = 10f))
        {
            place = hit.point + Vector3.up * distanceFromGround;
            if (hit.transform.gameObject.layer == 3 || hit.transform.gameObject.layer == 7)
            {
                if (ghostObjectSpawned == false)
                {
                    if (hit.transform.gameObject.layer == 3)
                    {
                        Instantiate(structureGhost, place, Quaternion.identity);
                    }
                    if (hit.transform.gameObject.layer == 7)
                    {
                        Instantiate(structureGhost, hit.transform.position, Quaternion.identity);
                    }
                    targets = GameObject.FindGameObjectsWithTag("StructureGhost");

                    for (int i = 0; i < targets.Length; i++)
                    {
                        distance = Vector3.Distance(this.transform.position, targets[i].transform.position);

                        if (distance < nearestDistance)
                        {
                            nearestTarget = targets[i];
                            nearestDistance = distance;
                        }
                    }
                    tempGhost = nearestTarget;
                    ghostObjectSpawned = true;
                }
                if (tempGhost != null)
                {
                    if (hit.transform.gameObject.layer == 3 || hit.transform.gameObject.layer == 7)
                    {
                        tempGhost.transform.position = place;
                        if (Input.GetKey(KeyCode.R))
                        {
                            tempGhost.transform.Rotate(Vector3.up * Time.deltaTime * 60);
                        }
                    }
                    if (wallItem)
                    {
                        if (hit.transform.gameObject.layer == 7)
                        {
                            if (hit.transform.gameObject.name == "Forward")
                            {
                                tempGhost.transform.position = hit.transform.position + Vector3.forward * gridDistance;
                            }
                            if (hit.transform.gameObject.name == "Right")
                            {
                                tempGhost.transform.position = hit.transform.position + Vector3.right * gridDistance;
                            }
                            if (hit.transform.gameObject.name == "Left")
                            {
                                tempGhost.transform.position = hit.transform.position + Vector3.left * gridDistance;
                            }
                            if (hit.transform.gameObject.name == "Back")
                            {
                                tempGhost.transform.position = hit.transform.position + Vector3.back * gridDistance;
                            }
                            if (hit.transform.gameObject.name == "Up")
                            {
                                tempGhost.transform.position = hit.transform.position + Vector3.up * gridDistance;
                            }
                            if (hit.transform.gameObject.name == "Down")
                            {
                                tempGhost.transform.position = hit.transform.position + Vector3.down * gridDistance;
                            }

                        }
                    }

                }
                if (Input.GetMouseButtonDown(0))
                {
                    ShootServerRpc(tempGhost.transform.position, tempGhost.transform.rotation);
                }
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        //if (wallHand)
        //{
        //    if (player.tag == "RedPlayer")
        //    {
        //        strucutre = redWall;
        //    }
        //    if (player.tag == "BluePlayer")
        //    {
        //        strucutre = blueWall;
        //    }
        //}
        //if (sentryHand)
        //{
        //    if (player.tag == "PlayerRed")
        //    {
        //        strucutre = redSentry;
        //    }
        //    if (player.tag == "PlayerBlue")
        //    {
        //        strucutre = blueSentry;
        //    }
        //}
        GameObject bullet = Instantiate(strucutre, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
        BuildClientRpc();
        GetComponent<NetworkObject>().Despawn();
    }
    [ClientRpc]
    void BuildClientRpc()
    {
        Holder holder = transform.parent.transform.parent.GetComponent<Holder>();
        holder.isStructureHolderFull = false;
    }
}
