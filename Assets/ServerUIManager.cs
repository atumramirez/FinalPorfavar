using Unity.Netcode;
using UnityEngine;

public class ServerUIManager : NetworkBehaviour
{
    [SerializeField] private GameObject RollMenu;
    [SerializeField] private GameObject JunctionMenu;
    public void ShowRollButton()
    {
        ShowRollButtonClientRpc();
    }

    public void ShowJunctionButtons()
    {
        ShowJunctionButtonsClientRpc();
    }

    public void HideAllButtons()
    {
        HideAllButtonsClientRpc();
    }

    //Network Stuff
    [ClientRpc]
    private void ShowRollButtonClientRpc()
    {
        RollMenu.SetActive(true);
        JunctionMenu.SetActive(false);
    }

    [ClientRpc]
    private void ShowJunctionButtonsClientRpc()
    {
        RollMenu.SetActive(false);
        JunctionMenu.SetActive(true);
    }

    [ClientRpc]
    private void HideAllButtonsClientRpc()
    {
        RollMenu.SetActive(false);
        JunctionMenu.SetActive(false);
    }
}
