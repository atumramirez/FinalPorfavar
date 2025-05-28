using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public int numberOfPlayers = 4;
    public TurnManager turnManager;
    public Transform parentTransform;

    void Start()
    {
        if (playerPrefab == null || turnManager == null)
        {
            Debug.LogError("PlayerPrefab or TurnManager not assigned.");
            return;
        }

        for (int i = 0; i < numberOfPlayers; i++)
        {

            GameObject playerObj = Instantiate(playerPrefab, parentTransform);
            playerObj.name = $"Jogador{i + 1}";

            PlayerController playerController = playerObj.GetComponent<PlayerController>();
            if (playerController != null)
            {
                turnManager.players.Add(playerController);
            }
            else
            {
                Debug.LogError($"Player prefab is missing PlayerController component.");
            }
        }
    }
}