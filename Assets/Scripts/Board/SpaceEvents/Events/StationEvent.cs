using UnityEngine;

[CreateAssetMenu(fileName = "New Station Space Event", menuName = "BoardGame/SpaceEvents/Station")]
public class StationEvent : SpaceEvent
{
    [SerializeField] private ServerUIManager serverUIManager;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        PlayerStats player = animator.GetComponent<PlayerStats>();

        if (player == null)
        {
            animator.Paused = false;
            return;
        }
    }
}