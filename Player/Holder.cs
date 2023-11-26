using Unity.Netcode;
using UnityEngine;

public class Holder : NetworkBehaviour
{
    public GameObject weaponHolderPart;
    public GameObject structureHolderPart;
    public GameObject spellHolderPart;
    public GameObject potionHolderPart;

    public bool isStructureHolderFull;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        Invoke(nameof(HideServerRpc), 1f);
    }
    private void Update()
    {
        if (!IsOwner) return;
        Select();
    }

    // ----------------------------------------> SELECT HOLDER <----------------------------------------
    void Select()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HolderServerRpc(true, false, false, false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HolderServerRpc(false, true, false, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HolderServerRpc(false, false, true, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HolderServerRpc(false, false, false, true);
        }
    }

    // ----------------------------------------> SET ACTIVE HOLDER <----------------------------------------
    [ServerRpc (RequireOwnership = false)]
    public void HolderServerRpc(bool weaponBool, bool structureBool, bool spellBool, bool potionBool)
    {
        HolderClientRpc(weaponBool, structureBool, spellBool, potionBool);
    }
    [ClientRpc]
    void HolderClientRpc(bool weaponBool, bool structureBool, bool spellBool, bool potionBool)
    {
        weaponHolderPart.SetActive (weaponBool);
        structureHolderPart.SetActive (structureBool);
        spellHolderPart.SetActive (spellBool);
        potionHolderPart.SetActive (potionBool);
    }
    // ----------------------------------------> HIDE ALL HOLDERS <----------------------------------------
    [ServerRpc(RequireOwnership = false)]
    void HideServerRpc()
    {
        HideClientRpc();
    }
    [ClientRpc]
    void HideClientRpc()
    {
        weaponHolderPart.SetActive(false);
        structureHolderPart.SetActive(false);
        spellHolderPart.SetActive(false);
        potionHolderPart.SetActive(false);
    }
}
