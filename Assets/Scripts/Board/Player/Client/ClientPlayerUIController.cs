using UnityEngine;
using Unity.Netcode;
using System.Numerics;

public class ClientPlayerUIController : NetworkBehaviour
{
    [SerializeField] private ServerUIManager serverUIManager;

    public void RollDiceButton()
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

    public void ExitShopPrompt()
    {
        ExitShopPromptServerRpc();
    }

    //Server Stuff
    [ServerRpc(RequireOwnership = false)]
    private void RollDiceServerRpc()
    {
        GameObject playerObj = GameObject.FindWithTag("PlayerTag");
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out ServerPlayerController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.RollDice();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeJunctionServerRpc(int direction)
    {
        GameObject playerObj = GameObject.FindWithTag("PlayerTag");
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out ServerPlayerController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.ChangeJunction(direction);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ConfirmJuctionServerRpc()
    {
        GameObject playerObj = GameObject.FindWithTag("PlayerTag");
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out ServerPlayerController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.ConfirmJuction();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ExitShopPromptServerRpc()
    {
        GameObject playerObj = GameObject.FindWithTag("PlayerTag");
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out ServerPlayerController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.ContinueMovement();
            serverUIManager.HideShopPromptUI();
        }
    }
}
