using UnityEngine;
using Unity.Netcode;

public class ClientController : NetworkBehaviour
{
    [SerializeField] private GameObject PlayerMenu;
    [SerializeField] private GameObject RollMenu;
    [SerializeField] private GameObject ItemMenu;
    [SerializeField] private GameObject MapMenu;

    [SerializeField] private SwipeToRoll swipeToRoll;

    [SerializeField] private ItemLogic itemLogic;

    [Header("Shop")]
    [SerializeField] private GameObject ShopMenu;


    public void PressRoll()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(true);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(false);
        swipeToRoll.Idle();
    }

    public void PressItem()
    {
        Debug.Log("Item Pressed");
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);
        MapMenu.SetActive(false);

        GetIdsServerRpc();
    }

    public void PressMap()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(true);
    }

    public void PressBack()
    {
        PlayerMenu.SetActive(true);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(false);
        ShopMenu.SetActive(true);
    }

    [ClientRpc]
    private void OpenMenuClientRpc(int[] ids)
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

            OpenMenuClientRpc(ids);
        }
    }

}
