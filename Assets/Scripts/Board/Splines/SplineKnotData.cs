using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class SplineKnotData : MonoBehaviour
{
    public SplineKnotIndex knotIndex;

    [HideInInspector] public UnityEvent<int> OnLand;

    public int coinGain = 3;

    [SerializeField] private bool pauseMovement = false;
    [SerializeField] public bool skipStepCount = false;

    [Header("Event")]
    [SerializeField] public SpaceEvent spaceEvent;

    private void OnValidate()
    {

    }

    public void EnterKnot(SplineKnotAnimate splineKnotAnimator, PlayerStats playerStats)
    {
        splineKnotAnimator.Paused = pauseMovement;

        if (spaceEvent != null)
            spaceEvent.StartEvent(splineKnotAnimator);

    }

    public void Land(PlayerStats playerStats)
    {
        
    }
}
