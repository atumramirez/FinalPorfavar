using UnityEngine;

public class WallSpace : SpaceEvent
{
    [Header("Wall")]
    public bool isClosed = false;
    public int wallCost = 15;

    private WallLogic wallLogic;

    [SerializeField] private bool pauseMovement = true;
    public bool skipStepCount = true;

    [SerializeField] private SpaceType spaceType = SpaceType.Passing;

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
        player.TryGetComponent<PlayerStats>(out var stats);
        stats.RemoveCoins(wallCost);
        player.ContinueMovement();
    }

    public override bool PauseMovement()
    {
        return pauseMovement;
    }

    public override bool SkipStepCount()
    {
        return skipStepCount;
    }

    public override SpaceType GetSpaceType()
    {
        return spaceType;
    }
}