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

    //Drop funtion
    [SerializeField] GameObject itemToDrop;
    [SerializeField] Transform droper;
    [SerializeField] StructureHolder structureHolder;

    [SerializeField] bool wallHand;
    [SerializeField] bool sentryHand;
    [SerializeField] bool elevatorHand;
    [SerializeField] bool tokenGeneratorHand;
    [SerializeField] bool Structurecrafter;
    [SerializeField] bool Potioncrafter;
    [SerializeField] bool Spellcrafter;

    private void OnEnable()
    {
        if (!IsOwner) return;
        nearestDistance = 10000;
        ghostObjectSpawned = false;

        if (wallHand)
        {
            if (!structureHolder.wI)
            {
                HideServerRpc(false);
            }
        }
        if (sentryHand)
        {
            if (!structureHolder.sI)
            {
                HideServerRpc(false);
            }
        }
        if (elevatorHand)
        {
            if (!structureHolder.eI)
            {
                HideServerRpc(false);
            }
        }
        if (tokenGeneratorHand)
        {
            if (!structureHolder.tGI)
            {
                HideServerRpc(false);
            }
        }
        if (Structurecrafter)
        {
            if (!structureHolder.stCI)
            {
                HideServerRpc(false);
            }
        }
        if (Potioncrafter)
        {
            if (!structureHolder.poCI)
            {
                HideServerRpc(false);
            }
        }
        if (Spellcrafter)
        {
            if (!structureHolder.spCI)
            {
                HideServerRpc(false);
            }
        }

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
                    if(hit.transform.gameObject.layer == 3)
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
                    if (hit.transform.gameObject.layer == 3)
                    {
                        tempGhost.transform.position = place;
                    }
                    if (wallHand)
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
                    ShootServerRpc(tempGhost.transform.position, Quaternion.identity);
                    FPServerRpc();
                    HideServerRpc(false);
                }
            }
        }
        //Drop function
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropServerRpc(droper.position, gameObject.transform.rotation);
            FPServerRpc();
            HideServerRpc(false);
        }

    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(strucutre, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc(RequireOwnership = false)]
    void DropServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(itemToDrop, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc(bool gunCannonB)
    {
        HideClientRpc(gunCannonB);
    }
    [ClientRpc]
    void HideClientRpc(bool gunCannonB)
    {
        gameObject.SetActive(gunCannonB);
    }

    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        if (wallHand)
        {
            structureHolder.wI = false;
        }
        if (sentryHand)
        {
            structureHolder.sI = false;
        }
        if (elevatorHand)
        {
            structureHolder.eI = false;
        }
        if (tokenGeneratorHand)
        {
            structureHolder.tGI = false;
        }
        if (Structurecrafter)
        {
            structureHolder.stCI = false;
        }
        if (Potioncrafter)
        {
            structureHolder.poCI = false;
        }
        if (Spellcrafter)
        {
            structureHolder.spCI = false;
        }
    }
}
