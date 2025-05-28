using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ItemLogic : NetworkBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button[] itemButtons;
    [SerializeField] private Button confirmButton;

    private int SelectedIndex = 0;

    public ItemDatabase itemDatabase;    

    public void OpenMenu(int[] itemIds)
    {
        Debug.Log("Menu Opened");
        menuPanel.SetActive(true);

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < itemIds.Length)
            {
                itemButtons[i].interactable = true;

                int index = i;
                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(index, itemIds[i]));
            }
            else
            {
                itemButtons[i].interactable = false;
                itemButtons[i].onClick.RemoveAllListeners();
            }
        }
    }

    void OnItemButtonClicked(int index, int id)
    {
        SelectedIndex = index;
        Debug.Log($"Selected item: {itemDatabase.GetItemName(id)}");
    }

    void UpdateUI(int index)
    {
        if (index >= 0)
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
    }

    public void OnConfirm()
    {
        OnCofirmServerRpc(SelectedIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCofirmServerRpc(int SelectedIndex, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            Debug.Log("Usaste Item");
            if (playerObj.TryGetComponent(out PlayerStats stats))
            {
                controller.asUsedItem = true;
                controller.usedItemId = stats.inventory[SelectedIndex].Id;
            }
        }
    }

}
