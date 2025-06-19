using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private int NumRounds;
    [SerializeField] private NumberRoundsUI NumberRoundsUI;

    [SerializeField] private MinigameManager minigameManager;
    private bool waitingForMinigame = false;

    public List<PlayerController> players = new();
    public int currentPlayerIndex { get; private set; } = 0;
    public int currentPlayer = 0;
    public int currentRound { get; private set; } = 0;

    public CameraFollow cameraFollow;

    private int totalRounds = 20;

    public PlayerController CurrentPlayer => players[currentPlayerIndex];

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

        NumberRoundsUI.GetTotalRounds(totalRounds);
        StartRound();
    }

    private void StartRound()
    {
        currentRound++;
        NumberRoundsUI.UpdateText(currentRound);
        currentPlayerIndex = 0;
        currentPlayer = currentPlayerIndex + 1;
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
        Debug.Log("Turno acabado");
        if (!turnInProgress) return;

        turnInProgress = false;

        currentPlayerIndex++;
        currentPlayer = currentPlayerIndex + 1;

        if (currentPlayerIndex >= players.Count)
        {
            if (minigameManager != null)
            {
                waitingForMinigame = true;
                minigameManager.OnMinigameEnd.AddListener(OnMinigameComplete);
                minigameManager.StartMinigame();
            }
            else
            {
                StartRound(); 
            }
        }
        else
        {
            StartPlayerTurn();
        }
    }

    private void OnMinigameComplete()
    {
        minigameManager.OnMinigameEnd.RemoveListener(OnMinigameComplete);
        waitingForMinigame = false;
        StartRound(); 
    }
}