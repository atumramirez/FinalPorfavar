using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public List<ServerPlayerController> players = new List<ServerPlayerController>();
    public int currentPlayerIndex { get; private set; } = 0;
    public int currentRound { get; private set; } = 0;

    public CameraFollow cameraFollow;

    public ServerPlayerController CurrentPlayer => players[currentPlayerIndex];

    public UnityEvent<int> OnTurnStart; 
    public UnityEvent<int> OnRoundStart; 

    private bool turnInProgress = false;

    void Start()
    {
        if (players.Count == 0)
        {
            Debug.LogError("No players assigned to TurnManager.");
            return;
        }

        StartRound();
    }

    private void StartRound()
    {
        currentRound++;
        currentPlayerIndex = 0;
        OnRoundStart?.Invoke(currentRound);
        Debug.Log("Round: " + currentRound);
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        turnInProgress = true;

        for (int i = 0; i < players.Count; i++)
            players[i].enabled = (i == currentPlayerIndex);

        if (cameraFollow != null && players[currentPlayerIndex] != null)
            cameraFollow.SetTarget(players[currentPlayerIndex].transform);

        players[currentPlayerIndex].isReady = false;
        players[currentPlayerIndex].asRolled = false;

        OnTurnStart?.Invoke(currentPlayerIndex);
        Debug.Log((currentPlayerIndex + 1) + "'S Turn");
    }

    public void EndPlayerTurn()
    {
        if (!turnInProgress) return;

        turnInProgress = false;

        currentPlayerIndex++;

        if (currentPlayerIndex >= players.Count)
        {
            StartRound(); 
        }
        else
        {
            StartPlayerTurn(); 
        }
    }
}