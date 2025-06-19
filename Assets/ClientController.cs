using UnityEngine;
using Unity.Netcode;

public class ClientController : NetworkBehaviour
{
    [SerializeField] private GameObject PlayerMenu;
    [SerializeField] private GameObject RollMenu;
    [SerializeField] private GameObject ItemMenu;


    [SerializeField] private ItemLogic itemLogic;

    [SerializeField] private TurnManager TurnManager;

    public void PressRoll()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(true);
        ItemMenu.SetActive(false);
    }

    public void PressItem()
    {
        Debug.Log("Item Pressed");
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);

        GetIdsServerRpc();
    }

    public void PressBack()
    {
        PlayerMenu.SetActive(true);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        PlayerMenu.SetActive(true);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
    }

    public void OpenClientMenu(int[] ids)
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { (ulong)TurnManager.currentPlayer }
            }
        };

        OpenMenuClientRpc(ids, clientRpcParams);
    }

    [ClientRpc]
    private void OpenMenuClientRpc(int[] ids, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Menu Almost Opened");
        itemLogic.OpenMenu(ids);
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetIdsServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");

        if (playerObj != null && playerObj.TryGetComponent(out PlayerStats stats))
        {
            int[] ids = new int[stats.inventory.Count];
            for (int i = 0; i < stats.inventory.Count; i++)
            {
                ids[i] = stats.inventory[i].Id;
            }
            Debug.Log(ids);
            OpenClientMenu(ids);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void HasItemServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");

        bool Bolivia = false;

        if (playerObj != null && playerObj.TryGetComponent(out PlayerController playerController))
        {
            playerObj.TryGetComponent(out PlayerStats stats);
            if (stats.inventory.Count > 0 || !playerController.asUsedItem)
            {
                Bolivia = true;
            }

        }
    }

}
