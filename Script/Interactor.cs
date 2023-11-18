using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : NetworkBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
                if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
    }
}
