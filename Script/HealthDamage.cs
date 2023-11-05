using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthDamage : NetworkBehaviour
{
    bool gotDamaged = false;
    public NetworkVariable<int> healthPoint = new NetworkVariable<int>();
    public float hp = 40f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        healthPoint.Value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoint.Value <= 0)
        {
            Debug.Log("You died" + GetComponent<HealthDamage>().healthPoint.Value);
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        //if (!IsOwner) { return; }
        //if (collision.gameObject.GetComponent<Bullet>())
        //{
        //    if(!gotDamaged)
        //    {
        //        GetDamageServerRpc();
        //        Debug.Log("Your health is " + healthPoint.Value);
        //        gotDamaged = true;
        //        Debug.Log(gotDamaged);
        //        Invoke(nameof(gotDamage), 0.1f);
        //        if (healthPoint.Value <= 0)
        //        {
        //            Debug.Log("You died");
        //        }
        //    }
        //}


    }
    [ServerRpc(RequireOwnership = false)]
    public void GetDamageServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            NetworkObject player = client.PlayerObject;
            player.GetComponent<HealthDamage>().healthPoint.Value -= 10;
        }
    }
    void gotDamage()
    {
        gotDamaged = false;
        Debug.Log(gotDamaged);
    }
    //IEnumerator Cooldown(NetworkObject player)
    //{
    //    TagsManager tagsManager = player.GetComponent<TagsManager>();
    //    var script = player.GetComponent<Movement>();
    //    script.enabled = false;
    //    yield return new WaitForSeconds(0.01f);
    //    if (tagsManager.tagsList.Contains("Blue"))
    //    {
    //        gameObject.transform.position = new Vector3(-100f, 3f, 100f);
    //    }
    //    else if (tagsManager.tagsList.Contains("Red"))
    //    {
    //        gameObject.transform.position = new Vector3(-100f, 3f, -100f);
    //    }
    //    Debug.Log("Odliczam");
    //    yield return new WaitForSeconds(2f);
    //    Debug.Log("Odpalono");
    //    script.enabled = true;
    //}
    //[ClientRpc]
    //void HandleCollisionClientRpc(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Bullet>())
    //    {
    //        Destroy(gameObject);
    //    }

    //}
}
