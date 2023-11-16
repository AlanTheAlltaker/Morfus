using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] Button startHostButton;
    [SerializeField] Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            Hide();

        });
        startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            NetworkManager.Singleton.StartClient();
            Hide();

        });
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
