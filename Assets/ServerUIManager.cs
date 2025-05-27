using Unity.Netcode;
using UnityEngine;
using UnityEngine.Splines;

public class ServerUIManager : NetworkBehaviour
{
    ///


    /// <summary>
    /// Menu Cliente
    /// </summary>
    [SerializeField] private GameObject ReadyMenu;
    [SerializeField] private GameObject ClientMenu;
    [SerializeField] private GameObject StationPromptMenu;

    /// <summary>
    /// Menus
    /// </summary>
    [Header("Client Buttons")]
    [SerializeField] private GameObject PlayerMenu;
    [SerializeField] private GameObject RollMenu;

    [Header("Junction")]
    [SerializeField] private GameObject JunctionMenu;

    [Header("Shop")]
    [SerializeField] private GameObject ShopPromptMenu;

    [Header("Trap")]
    [SerializeField] private GameObject TrapPromptMenu;

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

    public void ShowShopPromptUI()
    {
        ShowShopPromptUIClientRpc();
    }
    public void HideShopPromptUI()
    {
        HideShopPromptUIClientRpc();
    }

    /// <summary>
    /// 
    /// </summary>

    [ClientRpc]
    private void ShowReadyMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(true);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
        ShopPromptMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowClientMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(true);
        JunctionMenu.SetActive(false);
        ShopPromptMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowJunctionButtonsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(true);
        ShopPromptMenu.SetActive(false);
    }

    [ClientRpc]
    private void HideAllButtonsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowShopPromptUIClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
        ShopPromptMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideShopPromptUIClientRpc()
    {
        ShopPromptMenu.SetActive(false);
    }


}
