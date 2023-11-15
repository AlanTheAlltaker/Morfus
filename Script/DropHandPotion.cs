using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class DropHandPotion : NetworkBehaviour
{
    [SerializeField] GameObject itemToDrop;
    [SerializeField] Transform droper;
    [SerializeField] PotionHolder potionHolder;

    [SerializeField] bool isInvisiblePotion;
    [SerializeField] bool isHealingPotion;
    private void OnEnable()
    {
        if (isInvisiblePotion)
        {
            if (!potionHolder.iP)
            {
                HideServerRpc();
            }
        }
        if (isHealingPotion)
        {
            if (!potionHolder.hP)
            {
                HideServerRpc();
            }
        }
    }
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShootServerRpc(droper.position, gameObject.transform.rotation);
            FPServerRpc();
            HideServerRpc();
        }
    }

    //Drop item
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(itemToDrop, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    //Set false
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        if (isInvisiblePotion)
        {
            potionHolder.iP = false;
        }
        if (isHealingPotion)
        {
            potionHolder.hP = false;
        }
    }

    //Hide
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc()
    {
        HideClientRpc();
    }
    [ClientRpc]
    void HideClientRpc()
    {
        gameObject.SetActive(false);
    }
}
