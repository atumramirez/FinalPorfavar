using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.Splines;

public class SplineKnotData : NetworkBehaviour
{
    public SplineKnotIndex knotIndex;

    [HideInInspector] public UnityEvent<int> OnLand;

    [SerializeField] private bool pauseMovement = false;
    [SerializeField] public bool skipStepCount = false;

    private enum SpaceType {None, Full, Passing}
    [SerializeField] private SpaceType spaceType = SpaceType.Full;

    [Header("Event")]
    [SerializeField] public SpaceEvent spaceEvent;

    public void EnterKnot(SplineKnotAnimate splineKnotAnimator, PlayerStats playerStats)
    {
        splineKnotAnimator.Paused = pauseMovement;

        if (spaceEvent != null && spaceType == SpaceType.Passing)

            spaceEvent.StartEvent(splineKnotAnimator);
    }

    public void Land(SplineKnotAnimate splineKnotAnimator, PlayerStats playerStats)
    {
        if (spaceEvent != null && spaceType == SpaceType.Full)

            spaceEvent.StartEvent(splineKnotAnimator);
    }
}
