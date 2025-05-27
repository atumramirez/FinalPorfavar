using UnityEngine;
using Unity.Netcode;
using UnityEngine.Splines;
using UnityEditor;

public class StationLogic : NetworkBehaviour
{
    [SerializeField] private GameObject StationMenu;

    private int currentKnotIndex;
    private int currentSplineIndex;
    private SplineKnotAnimate currentAnimator;

    public void OpenMenu(int splineIndex, int knotIndex)
    {
        SaveValues(splineIndex, knotIndex);

        OpenMenuClientRpc(splineIndex, knotIndex);
    }

    public void OnConfirm()
    {
        OnConfirmServerRpc(currentSplineIndex, currentKnotIndex);
    }

    public void OnCancel()
    {

    }

    public void SaveValues(int spline, int knot)
    {
        currentSplineIndex = spline;
        currentKnotIndex = knot;
    }

    [ClientRpc]
    private void OpenMenuClientRpc(int splineIndex, int knotIndex)
    {
        SaveValues(splineIndex, knotIndex);
        StationMenu.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnConfirmServerRpc(int splineIndex, int knotIndex, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out SplineKnotAnimate controller))
        {
            controller.TeleportToKnot(splineIndex, knotIndex);
        }

        if (playerObj != null && playerObj.TryGetComponent(out ServerPlayerController playerController))
        {
            playerController.EndTurn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCancelServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        string playerTag = $"Jogador{clientId}";

        GameObject playerObj = GameObject.Find(playerTag);
        Debug.Log("Procurar objeto.");
        if (playerObj != null && playerObj.TryGetComponent(out ServerPlayerController playerController))
        {
            playerController.EndTurn();
        }
    }
}
