using UnityEngine;
using Unity.Netcode;

public class TrapLogic : NetworkBehaviour
{
    [SerializeField] private GameObject TrapMenu;

    [SerializeField] private TurnManager TurnManager;

    public void OpenClient()
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { (ulong)TurnManager.currentPlayer }
            }
        };

        OpenClientRpc(clientRpcParams);
    }

    public void OnConfirm()
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { (ulong)TurnManager.currentPlayer }
            }
        };

        TrapMenu.SetActive(false);
        OnConfirmServerRpc();
    }

    public void OnCancel()
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { (ulong)TurnManager.currentPlayer }
            }
        };

        TrapMenu.SetActive(false);
        OnCancelServerRpc();
    }

    public void HideMenu()
    {
        HideMenuClientRpc();
    }

    [ClientRpc]
    private void OpenClientRpc(ClientRpcParams clientRpcParams = default)
    {
        TrapMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        TrapMenu.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnConfirmServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("On confirm");
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.BuyTrap();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCancelServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("On confirm");
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.EndTurn();
        }
    }
}
