using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CannonTest : NetworkBehaviour, IInteractable
{
    bool turnedOn = false;
    [SerializeField] float speed = 3f;
    [SerializeField] Transform cannon;

    bool isCooldown = false;
    [SerializeField] float cooldownTime;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    public void Interact()
    {
        SetParentServerRpc();
    }
    void Update()
    {
        if (!IsOwner) return;
        if (turnedOn)
        {
            if (Input.GetKey(KeyCode.W))
            {
                cannon.Rotate(Vector3.right * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                cannon.Rotate(Vector3.left * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.down * Time.deltaTime * speed);
            }
            if (Input.GetKeyDown(KeyCode.F) && !isCooldown)
            {
                StartCoroutine(Cooldown());
                ShootServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
        }
    }
    // SET PARENT
    [ServerRpc(RequireOwnership = false)]
    void SetParentServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            gameObject.GetComponent<NetworkObject>().ChangeOwnership(clientId);
            NetworkObject target = client.PlayerObject.GetComponent<NetworkObject>();
            SetParentClientRpc(target);
        }
    }
    [ClientRpc]
    void SetParentClientRpc(NetworkObjectReference target)
    {
        target.TryGet(out NetworkObject playerNet);
        Transform player = playerNet.GetComponent<Transform>();
        Movement movement = player.gameObject.GetComponent<Movement>();
        if (!turnedOn)
        {
            movement.enabled = false;
            turnedOn = true;
        } else if (turnedOn)
        {
            movement.enabled = true;
            turnedOn = false;
        }
        
    }
    // SHOOT
    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
