using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button leaveShopButton;
    [SerializeField] private List<Button> itemButtons; // 3 Buttons with Image component for icons

    [Header("Items")]
    [SerializeField] private List<Item> allAvailableItems;

    private List<Item> currentShopItems = new List<Item>();
    private PlayerStats currentBuyer;
    private Action onShopClosed;

    private int selectedIndex = -1;

    public void OpenShop(PlayerStats player, Action onCloseCallback)
    {
        currentBuyer = player;
        onShopClosed = onCloseCallback;
        gameObject.SetActive(true);

        SetupShopItems();
        UpdateUI(-1); // No item selected at start

        leaveShopButton.onClick.RemoveAllListeners();
        leaveShopButton.onClick.AddListener(CloseShop);

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            if (selectedIndex >= 0 && selectedIndex < currentShopItems.Count)
            {
                Item selectedItem = currentShopItems[selectedIndex];
                if (currentBuyer.TryBuyItem(selectedItem))
                {
                    Debug.Log("Bought: " + selectedItem.itemName);
                    CloseShop();
                }
            }
        });
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

            buyButton.interactable = currentBuyer.Coins >= item.price;
        }
        else
        {
            descriptionText.text = "Select an item to see its details.";
            buyButton.interactable = false;
        }
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        onShopClosed?.Invoke(); // Resume movement
        Destroy(gameObject);
    }
}