using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlyingPotion : NetworkBehaviour
{
    [SerializeField] Flying fly;
    [SerializeField] Transform potionHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0))
        {
            fly.flyingEffect = true;
            transform.SetParent(null);
            GetComponent<FlyingPotion>().enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Flying>())
        {
            //fly = collision.gameObject.GetComponent<Flying>();

            //Transform player = collision.gameObject.transform;

            //Transform potionHolder = player.GetChild(0).gameObject.transform;

            transform.SetParent(potionHolder);
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localPosition = Vector3.zero;
            GetComponent<FlyingPotion>().enabled = true;
        }
    }

}
