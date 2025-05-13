using UnityEngine;
using Unity.Netcode;

public class UseDebug : NetworkBehaviour
{
    public void WriteLog(string text)
    {
        if (IsClient)
        {
            WriteLogServerRpc(text);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void WriteLogServerRpc(string text)
    {
        Debug.Log($"Server received log from client {OwnerClientId}: {text}");
    }
}