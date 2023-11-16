using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCleaner : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.position = new Vector3(-95f, 25f, 0f);
    }
}
