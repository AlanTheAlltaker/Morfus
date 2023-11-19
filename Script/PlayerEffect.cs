using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerEffect : NetworkBehaviour
{
    // Flying Effect
    public bool flyingEffect = false;
    [SerializeField] Movement move;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform player;

    // Invisible effect
    public bool invisibleEffect = false;
    [SerializeField] MeshRenderer playerRenderer;
    [SerializeField] MeshRenderer glassRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { return; }
        if (flyingEffect)
        {
            rb.AddForce(player.forward * 1f);
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(player.up * 5f);
            }
        }
        if (invisibleEffect)
        {
            InvisibleEffectServerRpc();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) { return; }
        FPServerRpc();
    }

    //Invisible effect
    [ServerRpc(RequireOwnership = false)]
    void InvisibleEffectServerRpc()
    {
        InvisibleEffectClientRpc();
    }
    [ClientRpc]
    void InvisibleEffectClientRpc()
    {
        StartCoroutine(Potion());
    }
    IEnumerator Potion()
    {
        playerRenderer.enabled = false;
        glassRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        playerRenderer.enabled = true;
        glassRenderer.enabled = true;
        invisibleEffect = false;
    }
    // SET FLYING FALSE
    [ServerRpc(RequireOwnership = false)]
    void FPServerRpc()
    {
        FPClientRpc();
    }
    [ClientRpc]
    void FPClientRpc()
    {
        flyingEffect = false;
    }
}
