using Unity.Netcode;
using UnityEngine;

public class ServerUIManager : NetworkBehaviour
{
    //Movement UI
    [SerializeField] private GameObject RollMenu;
    [SerializeField] private GameObject JunctionMenu;

    [SerializeField] private GameObject ShopPrompt;

    public void ShowRollButton()
    {
        ShowRollButtonClientRpc();
    }

    public void ShowJunctionButtons()
    {
        ShowJunctionButtonsClientRpc();
    }

    public void HideAllButtons()
    {
        HideAllButtonsClientRpc();
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
    private void ShowRollButtonClientRpc()
    {
        RollMenu.SetActive(true);
        JunctionMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowJunctionButtonsClientRpc()
    {
        RollMenu.SetActive(false);
        JunctionMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideAllButtonsClientRpc()
    {
        RollMenu.SetActive(false);
        JunctionMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowShopPromptUIClientRpc()
    {
        ShopPrompt.SetActive(true);
    }

    [ClientRpc]
    private void HideShopPromptUIClientRpc()
    {
        ShopPrompt.SetActive(false);
    }
}
