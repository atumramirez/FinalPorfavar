using UnityEngine;

public class ShopSpace : SpaceEvent
{
    [SerializeField] private ShopLogic shopLogic;
    public override void StartEvent(SplineKnotAnimate animator)
    {
        if (!animator.TryGetComponent<PlayerStats>(out var player))
        {
            animator.Paused = false;
            return;
        }

        shopLogic.OpenPromptMenu();
    }
}