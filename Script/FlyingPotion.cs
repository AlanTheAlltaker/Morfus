using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlyingPotion : NetworkBehaviour
{
    [SerializeField] Flying fly;
    [SerializeField] Movement movement;
    [SerializeField] PotionHolder potionHolder;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform droper;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0) && !movement.isGrounded)
        {
            fly.flyingEffect = true;
            FPServerRpc();
            HideServerRpc(false);

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShootServerRpc(droper.position, gameObject.transform.rotation);
            FPServerRpc();
            HideServerRpc(false);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
    public void Destroy()
    {
        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
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
        potionHolder.fP = false;
    }

}
