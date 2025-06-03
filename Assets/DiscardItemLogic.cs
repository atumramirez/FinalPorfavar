using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiscardItemLogic : NetworkBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private GameObject discardMenu;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button discardButton;
    [SerializeField] private Button[] itemButtons;

    private int SelectedIndex = 0;

    public void OpenMenu(int[] itemIds)
    {
        Debug.Log("Crazy");
        OpenMenuClientRpc(itemIds);
    }

    public void UpdateMenu(int[] itemIds)
    {
        discardMenu.SetActive(true);

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < itemIds.Length)
            {
                itemButtons[i].interactable = true;
                itemButtons[i].GetComponentInChildren<TMP_Text>().text = itemDatabase.GetItemName(itemIds[i]);

                int index = i;
                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(index, itemIds[index]));
            }
            else
            {
                itemButtons[i].interactable = false;
                itemButtons[i].GetComponentInChildren<TMP_Text>().text = "Empty";
                itemButtons[i].onClick.RemoveAllListeners();
            }
        }
    }

    public void OnItemButtonClicked(int index, int id)
    {
        SelectedIndex = index;
        UpdateUI(index, id);
    }

    public void UpdateUI(int index, int id)
    {
        if (index >= 0)
        {
            description.text = itemDatabase.GetItemDescription(id);
            discardButton.interactable = true;
        }
        else
        {
            description.text = "Select an Item";
            discardButton.interactable = false;
        }
    }

    public void DiscardItem()
    {
        DiscardItemServerRpc(SelectedIndex);
        CloseMenu();
    }

    public void CloseMenu()
    {
        discardMenu.SetActive(false);
    }

    [ClientRpc]
    private void OpenMenuClientRpc(int[] itemIds)
    {
        UpdateMenu(itemIds);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DiscardItemServerRpc(int SelectedIndex, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerStats player))
        {
            Debug.Log("Remover Item");
            player.RemoveItem(SelectedIndex);
        }
    }
}
