using Unity.Netcode;
using UnityEngine;

public class ShopLogic : NetworkBehaviour
{
    [SerializeField] private GameObject ShopPromptMenu;
    public void OpenPromptMenu()
    {
        OpenPromptMenuClientRpc();
    }

    public void OnConfirm()
    {
        
    }

    public void BuyButton()
    {
        BuyButtonServerRpc();
    }

    [ClientRpc]
    private void OpenPromptMenuClientRpc()
    {
        ShopPromptMenu.SetActive(true);
    }


    [ServerRpc(RequireOwnership = false)]
    private void BuyButtonServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("On confirm");
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerStats controller))
        {
        }
    }

}
