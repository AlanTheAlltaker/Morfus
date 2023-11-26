using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    [SerializeField] GameObject joinUI;
    [SerializeField] TMP_InputField inputCode;
    [SerializeField] TextMeshProUGUI showCode;
    string JoinCode;
 
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            showCode.text = joinCode;

            NetworkManager.Singleton.StartHost();

            joinUI.SetActive(false);

        } catch (RelayServiceException e)
        {
            Debug.LogException(e);
        }
    }
    public async void JoinRelay()
    {
        try
        {
            JoinCode = inputCode.text;
            Debug.Log("Joining Relay with " +  JoinCode);
            
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(JoinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            joinUI.SetActive(false);

        }
        catch (RelayServiceException e)
        {
            Debug.LogException(e);
        }
    }
}
