using UnityEngine;

public class WallSpace : SpaceEvent
{
    [Header("Wall")]
    public bool isClosed = false;
    public int wallCost = 15;

    [SerializeField] private WallLogic wallLogic;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        if (!animator.TryGetComponent<PlayerController>(out var currentPlayer))
        {
            animator.Paused = false;
            return;
        }

        if (!isClosed)
        {
            Debug.Log("Passa caralho");
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        wallLogic.OpenMenu();
    }

    public void PayCrazy(PlayerController player)
    {
        Debug.Log("Perdeu Playboy");
        player.ContinueMovement();
    }
}