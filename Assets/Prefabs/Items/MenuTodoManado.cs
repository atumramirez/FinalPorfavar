using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemMenu : MonoBehaviour
{
    public GameObject menuPanel;      
    public Button[] itemButtons;         
    private PlayerStats playerStats;     

    public void OpenMenu(PlayerStats stats)
    {
        playerStats = stats;
        menuPanel.SetActive(true);      

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < playerStats.inventory.Count)
            {
                Item item = playerStats.inventory[i];
                itemButtons[i].interactable = true;

                int index = i;  
                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(index));
            }
            else
            {
                itemButtons[i].GetComponentInChildren<Text>().text = "Empty";
                itemButtons[i].interactable = false;
                itemButtons[i].onClick.RemoveAllListeners();
            }
        }
    }

    void OnItemButtonClicked(int index)
    {
        Item selectedItem = playerStats.inventory[index];
        Debug.Log($"Selected item: {selectedItem.itemName}");

    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
}