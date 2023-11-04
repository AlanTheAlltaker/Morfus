using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthDamage : NetworkBehaviour
{
    [SerializeField] float hp = 100f; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) { return; }
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            hp -= 20f;
            Debug.Log("Your hp is " + hp);
            if (hp <= 0)
            {
                Debug.Log("You are dead");
                hp = 100f;
                StartCoroutine(Cooldown());
            }
        }
    }
    IEnumerator Cooldown()
    {
        TagsManager tagsManager = gameObject.GetComponent<TagsManager>();
        var script = gameObject.GetComponent<Movement>();
        script.enabled = false;
        yield return new WaitForSeconds(0.01f);
        if (tagsManager.tagsList.Contains("Blue"))
        {
            gameObject.transform.position = new Vector3(-100f, 3f, 100f);
        }
        else if (tagsManager.tagsList.Contains("Red"))
        {
            gameObject.transform.position = new Vector3(-100f, 3f, -100f);
        }
        Debug.Log("Odliczam");
        yield return new WaitForSeconds(2f);
        Debug.Log("Odpalono");
        script.enabled = true;
    }
}
