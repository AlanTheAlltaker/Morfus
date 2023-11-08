using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public GameObject BuildUI;
    //[SerializeField] GameObject cameraPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        BuildUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //BuildUI.SetActive(Input.GetKey(KeyCode.Tab));

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BuildUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            //var script = cameraPlayer.GetComponent<MouseRotation>();
            //script.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            BuildUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            //var script = cameraPlayer.GetComponent<MouseRotation>();
            //script.enabled = true;
        }
    }
}