using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class ButtonLogic : NetworkBehaviour
{
    [SerializeField] private Button actionButton;
    void Start()
    {
        actionButton.onClick.AddListener(TestServerRPC);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestServerRPC()
    {
        Debug.Log(OwnerClientId + "clicou no butão.");
    }
}
