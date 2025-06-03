using UnityEngine;

public class TrapSpace : SpaceEvent
{
    private TrapLogic TrapLogic;

    [Header("Trap")]
    public bool hasTrap = false;
    public PlayerController trapOwner;
    [SerializeField] private int trapCost = 10;
    [SerializeField] private int rewardCoins = 6;
    [SerializeField] private int damage = 20;

    [SerializeField] private bool pauseMovement = false;
    public bool skipStepCount = false;

    [SerializeField] private SpaceType spaceType = SpaceType.Full;

    private void Start()
    {
        string Tag = "TrapLogic";
        TrapLogic = GameObject.Find(Tag).GetComponent<TrapLogic>();
    }
    public override void StartEvent(SplineKnotAnimate animator)
    {
        Debug.Log("Evento Começado");
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
                currentPlayer.EndTurn();
            }
            else
            {
                currentPlayer.GetComponent<PlayerStats>().TakeDamage(damage);
                Debug.Log("You landed on an enemy trap and took damage!");
                currentPlayer.EndTurn();
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
        Debug.Log("Trap colocada");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.RemoveCoins(trapCost);
        hasTrap = true;
        trapOwner = player;
        player.EndTurn();
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