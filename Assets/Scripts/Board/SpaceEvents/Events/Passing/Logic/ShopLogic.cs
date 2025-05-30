using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogic : NetworkBehaviour
{
    [SerializeField] private GameObject ShopPromptMenu;
    [SerializeField] private GameObject ShopMenu;

    [Header("Items")]
    [SerializeField] private List<Item> allAvailableItemsIds;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button leaveShopButton;
    [SerializeField] private List<Button> itemButtons;

    [Header("Item")]
    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private TurnManager TurnManager;

    private List<Item> currentShopItemsIds = new();
    private int selectedId = -1;

    public void OpenPromptMenu()
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { (ulong)TurnManager.currentPlayer }
            }
        };

        OpenPromptMenuClientRpc(clientRpcParams);
    }

    public void OnConfirm()
    {
        ShopPromptMenu.SetActive(false);
        ChooseThreeItems();
    }

    public void OnCancel() 
    {
        ShopPromptMenu.SetActive(false);
        OnCancelServerRpc();
    }

    public void ChooseThreeItems()
    {
        currentShopItemsIds = allAvailableItemsIds.OrderBy(i => UnityEngine.Random.value).Take(3).ToList();

        UpdateDescription(-1);
        for (int i = 0; i < itemButtons.Count; i++)
        {
            int index = i;

            if (i < currentShopItemsIds.Count)
            {
                int Id = itemDatabase.GetId(currentShopItemsIds[i]);
                itemButtons[i].GetComponentInChildren<TMP_Text>().text = itemDatabase.GetItemName(Id);
                itemButtons[i].gameObject.SetActive(true);

                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(Id));
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }

        ShopMenu.SetActive(true);
    }

    void OnItemButtonClicked(int id)
    {
        selectedId = id;
        UpdateDescription(id);
    }

    void UpdateDescription(int id)
    {
        if (id >= 0)
        {
            description.text = itemDatabase.GetItemDescription(id);
            buyButton.interactable = true;
        }
        else
        {
            description.text = "Select an Item";
            buyButton.interactable = false;
        }
    }

    public void BuyButton()
    {
        BuyButtonServerRpc(selectedId);
    }

    public void CloseShop()
    {
        ShopMenu.SetActive(false);
        CloseServerRpc();
    }

    [ClientRpc]
    private void OpenPromptMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        ShopPromptMenu.SetActive(true);
    }

    [ClientRpc]
    private void HidePromptMenuClientRpc()
    {
        ShopPromptMenu.SetActive(true);
    }

    [ClientRpc]
    private void CloseShopClientRpc()
    {
        ShopMenu.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void BuyButtonServerRpc(int Id, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.BuyItem(Id);
            CloseShopClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CloseServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.ContinueMovement();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCancelServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.ContinueMovement();
        }
    }
}
