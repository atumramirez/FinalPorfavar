using Unity.Netcode;
using UnityEngine;
using UnityEngine.Splines;

public class ServerUIManager : NetworkBehaviour
{

    [SerializeField] private GameObject ReadyMenu;
    [SerializeField] private GameObject ClientMenu;

    [Header("Client Buttons")]
    [SerializeField] private GameObject PlayerMenu;
    [SerializeField] private GameObject RollMenu;

    [Header("Junction")]
    [SerializeField] private GameObject JunctionMenu;

    public void ShowReadyMenu(ulong targetClientId)
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { targetClientId }
            }
        };

        ShowReadyMenuClientRpc(clientRpcParams);
    }

    public void ShowClientMenu(ulong targetClientId)
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { targetClientId }
            }
        };

        ShowClientMenuClientRpc(clientRpcParams);
    }

    public void ShowJunctionButtons(ulong targetClientId)
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { targetClientId }
            }
        };
        ShowJunctionButtonsClientRpc(clientRpcParams);
    }

    public void HideAllButtons(ulong targetClientId)
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { targetClientId }
            }
        };
        HideAllButtonsClientRpc(clientRpcParams);
    }


    [ClientRpc]
    private void ShowReadyMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(true);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);

    }

    [ClientRpc]
    private void ShowClientMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(true);
        JunctionMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowJunctionButtonsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideAllButtonsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
    }
}
