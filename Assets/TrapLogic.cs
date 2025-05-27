using UnityEngine;
using Unity.Netcode;

public class TrapLogic : NetworkBehaviour
{
    [SerializeField] private GameObject Menu;
    private int spline;
    private int knot;
    public void Open(int splineIndex, int knotIndex)
    {
        OpenClientRpc(splineIndex, knotIndex);
    }

    public void OnConfirm()
    {
    }

    public void OnCancel()
    {
        
    }

    public void SaveValues(int splineIndex, int knotIndex)
    {
        spline = splineIndex;
        knot = knotIndex;
    }

    [ClientRpc]
    private void OpenClientRpc(int splineIndex, int knotIndex)
    {
        SaveValues(splineIndex,knotIndex);
        Menu.SetActive(true);
    }

}
