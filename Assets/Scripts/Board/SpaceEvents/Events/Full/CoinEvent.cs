using UnityEngine;

public class CoinSpace : SpaceEvent
{
    [SerializeField] private int coinGain = 10;

    [SerializeField] private bool pauseMovement = false;
    public bool skipStepCount = false;

    [SerializeField] private SpaceType spaceType = SpaceType.Full;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        Debug.Log("Evento Começado");
        if (!animator.TryGetComponent<PlayerController>(out var currentPlayer))
        {
            animator.Paused = false;
            return;
        }

        Debug.Log("Ganhaste Monedas");
        currentPlayer.TryGetComponent<PlayerStats>(out var playerStats);
        playerStats.AddCoins(coinGain);
        currentPlayer.EndTurn();
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