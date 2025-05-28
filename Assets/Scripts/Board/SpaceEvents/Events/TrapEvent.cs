using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TrapSpace : SpaceEvent
{
    [SerializeField] TrapLogic TrapLogic;

    [Header("Trap")]
    public bool hasTrap = false;
    public PlayerController trapOwner;
    public int trapCost = 5;
    public int rewardCoins = 3;
    public int damage = 2;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        Debug.Log("Evento Com´çado");
        if (!animator.TryGetComponent<PlayerController>(out var currentPlayer))
        {
            animator.Paused = false;
            return;
        }

        if (!hasTrap)
        {
            Debug.Log("TentaSeila");
            TryPlaceTrap(currentPlayer);
        }
        else 
        {
            if (trapOwner == currentPlayer)
            {
                currentPlayer.GetComponent<PlayerStats>().AddCoins(rewardCoins);
                Debug.Log("You landed on your trap and got coins!");
            }
            else
            {
                currentPlayer.GetComponent<PlayerStats>().TakeDamage(damage);
                Debug.Log("You landed on an enemy trap and took damage!");
            }
        }
    }

    public void TryPlaceTrap(PlayerController player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        if (stats.Coins >= trapCost)
        {
            TrapLogic.OpenClient();
        }
        else
        {
            Debug.Log("Not enough coins to place a trap.");
        }
    }

    public void PlaceTrap(PlayerController player)
    {
        Debug.Log("Trap loca");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.RemoveCoins(trapCost);
        hasTrap = true;
        trapOwner = player;
        player.EndTurn();
    }
}