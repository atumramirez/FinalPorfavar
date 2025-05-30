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

    public enum SpaceType {None, Full, Passing}
    [SerializeField] private SpaceType spaceType = SpaceType.Full;

    [Header("Event")]
    [SerializeField] public SpaceEvent spaceEvent;

    private void Start()
    {
       spaceEvent = gameObject.GetComponent<SpaceEvent>();
       pauseMovement = spaceEvent.PauseMovement();
       skipStepCount = spaceEvent.SkipStepCount();
    }

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
