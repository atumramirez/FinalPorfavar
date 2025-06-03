using UnityEngine;
using UnityEngine.Splines;

public class StationSpace : SpaceEvent
{
    [SerializeField] private StationLogic StationLogic;
    public int splineIndex;
    public int knotIndex;

    [SerializeField] private SpaceType spaceType = SpaceType.Full;

    private void Start()
    {
        string Tag = "StationLogic";

        StationLogic = GameObject.Find(Tag).GetComponent<StationLogic>();
    }

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

    public override SpaceType GetSpaceType()
    {
        return spaceType;
    }
}