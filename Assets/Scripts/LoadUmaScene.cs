using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkSceneLoader : NetworkBehaviour
{
    [SerializeField] private string sceneToLoad = "GameScene";

    // Call this from a UI button or game event
    public void TryLoadScene()
    {
        if (IsServer)
        {
            LoadSceneForAll(sceneToLoad);
        }
    }

    // Server loads the scene and tells all clients to do the same
    private void LoadSceneForAll(string sceneName)
    {
        // This uses Netcode's built-in scene management
        NetworkManager.Singleton.SceneManager.LoadScene(
            sceneName,
            LoadSceneMode.Single
        );
    }
}