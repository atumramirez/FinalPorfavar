using UnityEngine;
using System.Collections.Generic;

public class ItemSpace : SpaceEvent
{
    [SerializeField] private List<Item> allAvailableItems;
    private Item randomItem;

    [SerializeField] private bool pauseMovement = false;
    public bool skipStepCount = true;

    [SerializeField] private SpaceType spaceType = SpaceType.Passing;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        if (!animator.TryGetComponent<PlayerController>(out var currentPlayer))
        {
            animator.Paused = false;
            return;
        }

        animator.TryGetComponent<PlayerStats>(out var playerStats);

        randomItem = allAvailableItems[0];
        Debug.Log("Recebeste este item" + randomItem);
        playerStats.GetItem(randomItem);
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
