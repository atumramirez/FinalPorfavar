using Unity.Netcode;
using UnityEngine;

public class ServerUIManager : NetworkBehaviour
{
    ///
    [SerializeField] private TurnManager turnManager;

    /// <summary>
    /// Menu Cliente
    /// </summary>
    [SerializeField] private GameObject ReadyMenu;
    [SerializeField] private GameObject ClientMenu;

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
    [SerializeField] private GameObject ShopMenu;

    //[SerializeField] private GameObject ShopPrompt;

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

    /*
    public void HideShopPromptUI()
    {
        HideShopPromptUIClientRpc();
    }
    */

    /// <summary>
    /// 
    /// </summary>

    [ClientRpc]
    private void ShowReadyMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(true);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
        ShopMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowClientMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(true);
        JunctionMenu.SetActive(false);
        ShopMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowJunctionButtonsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(true);
        ShopMenu.SetActive(false);
    }

    [ClientRpc]
    private void HideAllButtonsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
        ShopMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowShopPromptUIClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ReadyMenu.SetActive(false);
        ClientMenu.SetActive(false);
        JunctionMenu.SetActive(false);
        ShopMenu.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
    /*
    [ClientRpc]
    private void HideShopPromptUIClientRpc()
    {
        ShopPrompt.SetActive(false);
    }
    */
}
