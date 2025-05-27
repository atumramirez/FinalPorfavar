using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TrapSpace : SpaceEvent
{
    [SerializeField] TrapLogic TrapLogic;

    public bool hasTrap = false;
    public ServerPlayerController trapOwner;
    public int trapCost = 5;
    public int rewardCoins = 3;
    public int damage = 2;

    [SerializeField] private GameObject CurrentSpace;


    [Header("Prompt")]
    [SerializeField] private ServerUIManager uIManager;


    public override void StartEvent(SplineKnotAnimate animator)
    {
        
        if (!animator.TryGetComponent<ServerPlayerController>(out var currentPlayer))
        {
            animator.Paused = false;
            return;
        }

        if (!hasTrap)
        {
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

    public void TryPlaceTrap(ServerPlayerController player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        if (stats.Coins >= trapCost)
        {
        }
        else
        {
            Debug.Log("Not enough coins to place a trap.");
        }
    }

    public void PlaceTrap(ServerPlayerController player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.RemoveCoins(trapCost);
        hasTrap = true;
        trapOwner = player;
    }
}