using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CoinSpace : SpaceEvent
{
    [SerializeField] private int coinGain;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        Debug.Log("Evento Começado");
        if (!animator.TryGetComponent<PlayerController>(out var currentPlayer))
        {
            animator.Paused = false;
            return;
        }

        Debug.Log("Ganhaste Monedas");
        animator.TryGetComponent<PlayerStats>(out var playerStats);
        playerStats.AddCoins(coinGain);
    }
}