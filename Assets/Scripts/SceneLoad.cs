using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    public string clientScene = "ClientScene";
    public string serverScene = "ServerScene";

    public void LoadScenes()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Carregada");
            LoadClientSceneClientRpc();
            LoadClientSceneServerRpc();
        }
    }


    [ClientRpc]
    private void LoadClientSceneClientRpc()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            SceneManager.LoadScene(clientScene, LoadSceneMode.Single);
        }
    }

    [ServerRpc]
    private void LoadClientSceneServerRpc()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            SceneManager.LoadScene(serverScene, LoadSceneMode.Single);
        }
    }
}