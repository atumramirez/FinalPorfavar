using UnityEngine;
using Unity.Netcode;
using System.Numerics;
using UnityEngine.Splines;

public class ClientPlayerUIController : NetworkBehaviour
{
    [SerializeField] private ServerUIManager serverUIManager;
    [SerializeField] private ClientController clientController;

    public void SetReady()
    {
        SetReadyServerRpc();
        clientController.OpenMenu();
    }

    public void RollDice()
    {
        RollDiceServerRpc();
    }

    public void ChangeJunctionButton(int direction)
    {
        ChangeJunctionServerRpc(direction);
    }

    public void ConfirmJuctionButton()
    {
        ConfirmJuctionServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.ConfirmTurn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RollDiceServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            controller.RollDice();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeJunctionServerRpc(int direction, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.ChangeJunction(direction);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ConfirmJuctionServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out PlayerController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.ConfirmJuction();
        }
    }
}
