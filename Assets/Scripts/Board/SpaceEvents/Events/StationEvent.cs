using UnityEngine;
using UnityEngine.Splines;
using Unity.Netcode;

[CreateAssetMenu(fileName = "New Station Space Event", menuName = "BoardGame/SpaceEvents/Station")]
public class StationEvent : SpaceEvent
{
    [SerializeField] private StationLogic StationLogic;
    [SerializeField] private int SplineIndex;
    [SerializeField] private int KnotIndex;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        PlayerStats player = animator.GetComponent<PlayerStats>();

        if (player == null)
        {
            animator.Paused = false;
            return;
        }

        StationLogic.OpenMenu(SplineIndex, KnotIndex);
    }
}