using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject objectToSpawn; 

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsOwner)
        {
            RequestSpawnServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        Vector3 spawnPosition = new Vector3(0, 1, 0);
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        NetworkObject netObj = spawnedObject.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(clientId);
    }
}