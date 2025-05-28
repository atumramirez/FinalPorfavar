using UnityEngine;
using UnityEngine.Splines;

public class StationSpace : SpaceEvent
{
    [SerializeField] private StationLogic StationLogic;
    public int splineIndex;
    public int knotIndex;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        PlayerStats player = animator.GetComponent<PlayerStats>();

        if (player == null)
        {
            animator.Paused = false;
            return;
        }

        StationLogic.OpenMenu();
    }
}