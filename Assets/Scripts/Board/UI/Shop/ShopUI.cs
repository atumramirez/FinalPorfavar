using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class ShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button leaveShopButton;
    [SerializeField] private List<Button> itemButtons;

    [SerializeField] private GameObject Menu;

    [Header("Items")]
    [SerializeField] private List<Item> allAvailableItems;

    private List<Item> currentShopItems = new List<Item>();
    private PlayerStats currentBuyer;
    private Action onShopClosed;

    private int selectedIndex = -1;

    public void OpenShop(PlayerStats player)
    {
        Menu.SetActive(true);
        currentBuyer = player;

        SetupShopItems();
        UpdateUI(-1);
    }

    private void SetupShopItems()
    {
        currentShopItems = allAvailableItems.OrderBy(i => UnityEngine.Random.value).Take(3).ToList();

        for (int i = 0; i < itemButtons.Count; i++)
        {
            int index = i;

            if (i < currentShopItems.Count)
            {
                Item item = currentShopItems[i];
                itemButtons[i].gameObject.SetActive(true);
                itemButtons[i].GetComponent<Image>().sprite = item.icon;

                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() =>
                {
                    selectedIndex = index;
                    UpdateUI(index);
                });
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateUI(int selected)
    {
        if (selected >= 0 && selected < currentShopItems.Count)
        {
            Item item = currentShopItems[selected];
            descriptionText.text = $"{item.itemName}\n\n{item.description}\n\nPrice: {item.price} coins";

            // Enable Buy if player has enough coins and inventory space
            bool canBuy = currentBuyer.Coins >= item.price && currentBuyer.HasInventorySpace();
            buyButton.interactable = canBuy;
        }
        else
        {
            descriptionText.text = "Select an item to see its details.";
            buyButton.interactable = false;
        }
    }

    private void OnBuyButtonPressed()
    {
        if (selectedIndex >= 0 && selectedIndex < currentShopItems.Count)
        {
            Item selectedItem = currentShopItems[selectedIndex];

            if (currentBuyer.TryBuyItem(selectedItem))
            {
                descriptionText.text = $"You bought {selectedItem.itemName}!";
                buyButton.interactable = false;
            }
            else
            {
                descriptionText.text = $"Not enough coins or inventory full for {selectedItem.itemName}.";
            }
        }
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        onShopClosed?.Invoke();
    }
}