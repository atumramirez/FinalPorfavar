using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.Splines;

public class SplineKnotData : NetworkBehaviour
{
    public SplineKnotIndex knotIndex;

    [HideInInspector] public UnityEvent<int> OnLand;

    [SerializeField] private bool pauseMovement = false;
    public bool skipStepCount = false;

    public enum SpaceType {None, Full, Passing}
    private readonly SpaceType spaceType = SpaceType.Full;

    private SpaceEvent spaceEvent;

    private void Start()
    {
       spaceEvent = gameObject.GetComponent<SpaceEvent>();
       pauseMovement = spaceEvent.PauseMovement();
       skipStepCount = spaceEvent.SkipStepCount();
    }

    public void EnterKnot(SplineKnotAnimate splineKnotAnimator)
    {
        splineKnotAnimator.Paused = pauseMovement;

        if (spaceEvent != null && spaceType == SpaceType.Passing)

            spaceEvent.StartEvent(splineKnotAnimator);
    }

    public void Land(SplineKnotAnimate splineKnotAnimator)
    {
        if (spaceEvent != null && spaceType == SpaceType.Full)

            spaceEvent.StartEvent(splineKnotAnimator);
    }
}
