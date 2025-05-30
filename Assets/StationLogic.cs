using UnityEngine;
using Unity.Netcode;
using UnityEngine.Splines;
using UnityEditor;

public class StationLogic : NetworkBehaviour
{
    [SerializeField] private GameObject StationMenu;

    [SerializeField] private TurnManager TurnManager;

    public void OpenMenu()
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { (ulong)TurnManager.currentPlayer }
            }
        };

        OpenMenuClientRpc(clientRpcParams);
    }

    public void OnConfirm()
    {
        OnConfirmServerRpc();
    }

    public void OnCancel()
    {
        OnCancelServerRpc();
    }

    [ClientRpc]
    private void OpenMenuClientRpc(ClientRpcParams clientRpcParams = default)
    {
        StationMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideMenuClientRpc()
    {
        StationMenu.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnConfirmServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.Teleport();
            HideMenuClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCancelServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.EndTurn();
            HideMenuClientRpc(); 
        }
    }
}
