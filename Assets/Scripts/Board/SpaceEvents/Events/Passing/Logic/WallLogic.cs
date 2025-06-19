using Unity.Netcode;
using UnityEngine;

public class WallLogic : NetworkBehaviour
{
    [SerializeField] private GameObject WallMenu;

    public void OpenMenu()
    {
        OpenMenuClientRpc();
    }

    public void OnConfirm()
    {
        OnConfirmServerRpc();
    }

    public void OnDeny()
    {
        OnDenyServerRpc();
    }

    public void HideMenu()
    {
        HideMenuClientRpc();
    }

    [ClientRpc]
    private void OpenMenuClientRpc()
    {
        WallMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideMenuClientRpc()
    {
        WallMenu.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnConfirmServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.PayWall();
            HideMenuClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnDenyServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.GoBack();
            HideMenuClientRpc();
        }
    }
}
