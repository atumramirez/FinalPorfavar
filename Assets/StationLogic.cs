using UnityEngine;
using Unity.Netcode;
using UnityEngine.Splines;
using UnityEditor;

public class StationLogic : NetworkBehaviour
{
    [SerializeField] private GameObject StationMenu;

    public void OpenMenu()
    {
        OpenMenuClientRpc();
    }

    public void OnConfirm()
    {
        OnConfirmServerRpc();
    }

    public void OnCancel()
    {

    }

    [ClientRpc]
    private void OpenMenuClientRpc()
    {
        StationMenu.SetActive(true);
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
        }
    }
}
