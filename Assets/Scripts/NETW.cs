using UnityEngine;
using Unity.Netcode;

public class CubeSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnCubeServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!IsServer) return;

        Vector3 spawnPos = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
        cube.GetComponent<NetworkObject>().Spawn();
    }
}