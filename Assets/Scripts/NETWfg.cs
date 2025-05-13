using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class SpawnButtonUI : NetworkBehaviour
{
    [SerializeField] private Button spawnButton;

    private CubeSpawner spawner;

    private void Start()
    {
        spawnButton.onClick.AddListener(OnButtonPressedServerRpc);

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsClient)
            {
                TryFindSpawner();
            }
        };
    }

    void TryFindSpawner()
    {
        spawner = Object.FindAnyObjectByType<CubeSpawner>();

        if (spawner == null)
        {
            Debug.LogWarning("CubeSpawner not found in current scene.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnButtonPressedServerRpc()
    {
        if (spawner != null && NetworkManager.Singleton.IsClient)
        {
            spawner.SpawnCubeServerRpc();
        }
        else
        {
            Debug.LogWarning("Spawner not yet found or client not connected.");
        }
    }
}