using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LoadSceneOnButton : NetworkBehaviour
{
    [SerializeField] private Button loadSceneButton;
    [SerializeField] private string sceneNameToLoad = "ClientScene"; 

    private void Start()
    {
        loadSceneButton.onClick.AddListener(OnLoadSceneClickedClientRpc);
    }

    [ClientRpc]
    private void OnLoadSceneClickedClientRpc()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                sceneNameToLoad,
                LoadSceneMode.Single
            );
        }
    }
}