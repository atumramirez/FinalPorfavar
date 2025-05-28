using UnityEngine;
using Unity.Netcode;

public class ShopLweaogic : NetworkBehaviour
{
    [SerializeField] private GameObject ShopPrompt;
    [SerializeField] private GameObject ShopMenu;
    public void ShowChoice()
    {
        ShowChoiceClientRpc();
    }

    public void OnConfirm()
    {

    }

    public void OnCancel()
    {

    }

    public void CloseShop()
    {
        
    }

    [ClientRpc]
    private void ShowChoiceClientRpc()
    {
        ShopPrompt.SetActive(true);
    }
}
