using UnityEngine;

public class WallSpace : SpaceEvent
{
    [Header("Wall")]
    [SerializeField] private bool isClosed = false;
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
        Check(currentPlayer);
    }

    public void Check(PlayerController currentPlayer)
    {
        currentPlayer.TryGetComponent<PlayerStats>(out var stats);

        if (!isClosed)
        {
            currentPlayer.ContinueMovement();
            Debug.Log("Passa caralho");
        }
        else
        {
            if (wallCost > stats.Coins)
            {
                Debug.Log("N tens guito caralho");
                Return(currentPlayer);
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        wallLogic.OpenMenu();
    }

    public void PayCrazy(PlayerController player)
    {
        Debug.Log("Pagado");
        player.TryGetComponent<PlayerStats>(out var stats);
        stats.RemoveCoins(wallCost);
        player.ContinueMovement();
    }

    public void Return(PlayerController player)
    {
        Debug.Log("Para tras sua loca");
        player.GoBackwards();
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

    public override void GetSpaceLogic()
    {
        string Tag = "WallLogic";
        wallLogic = GameObject.Find(Tag).GetComponent<WallLogic>();
    }
}