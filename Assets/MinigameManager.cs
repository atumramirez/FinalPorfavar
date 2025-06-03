using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public UnityEvent OnMinigameStart;
    public UnityEvent OnMinigameEnd;

    public void StartMinigame()
    {
        Debug.Log("Minigame started!");
        OnMinigameStart?.Invoke();

        StartCoroutine(SimulateMinigame());
    }

    private IEnumerator SimulateMinigame()
    {
        yield return new WaitForSeconds(5f); // Simulated minigame duration
        EndMinigame();
    }

    public void EndMinigame()
    {
        Debug.Log("Minigame ended!");
        OnMinigameEnd?.Invoke();
    }
}