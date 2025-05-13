using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using Unity.Netcode;
using UnityEngine.XR;

public class ServerPlayerController : NetworkBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private SplineKnotAnimate splineKnotAnimator;
    [SerializeField] private ServerUIManager serverUIManager;
    private SplineKnotInstantiate splineKnotData;
    
    private ServerPlayerController player;

    public GameObject rollMenu;

    [Header("Events")]
    [HideInInspector] public UnityEvent OnRollStart;
    [HideInInspector] public UnityEvent OnRollJump;
    [HideInInspector] public UnityEvent<int> OnRollDisplay;
    [HideInInspector] public UnityEvent OnRollEnd;
    [HideInInspector] public UnityEvent OnRollCancel;
    [HideInInspector] public UnityEvent<bool> OnMovementStart;
    [HideInInspector] public UnityEvent<int> OnMovementUpdate;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        splineKnotAnimator = GetComponent<SplineKnotAnimate>();

        splineKnotAnimator.OnDestinationKnot.AddListener(OnDestinationKnot);
        splineKnotAnimator.OnKnotEnter.AddListener(OnKnotEnter);
        splineKnotAnimator.OnKnotLand.AddListener(OnKnotLand);

        if (FindAnyObjectByType<SplineKnotInstantiate>() != null)
            splineKnotData = FindAnyObjectByType<SplineKnotInstantiate>();


        player = GetComponent<ServerPlayerController>();
    }

    //Controlls
    public void RollDice()
    {
        int randomNumber = Random.Range(1, 7);
        Debug.Log("Que Louco");
        splineKnotAnimator.Animate(randomNumber);
    }

    public void ChangeJunction(int direction)
    {
        splineKnotAnimator.AddToJunctionIndex(direction);
    }

    public void ConfirmJuction()
    {
        splineKnotAnimator.inJunction = false;
    }

    //Tile stuff
    private void OnDestinationKnot(SplineKnotIndex index)
    {
        SplineKnotData data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];
        if (data.skipStepCount)
            splineKnotAnimator.SkipStepCount = true;
    }

    private void OnKnotLand(SplineKnotIndex index)
    {
        SplineKnotData data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];

        StartCoroutine(DelayCoroutine());
        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(.08f);
            data.Land(stats);
            OnMovementStart.Invoke(false);
            yield return new WaitForSeconds(2);
        }
    }

    private void OnKnotEnter(SplineKnotIndex index)
    {
        SplineKnotData data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];
        data.EnterKnot(splineKnotAnimator, stats);
        OnMovementUpdate.Invoke(splineKnotAnimator.Step);
    }


    //Buttons
    void Update()
    {
        if (player == null || splineKnotAnimator == null) return;

        if (!splineKnotAnimator.isMoving && !splineKnotAnimator.inJunction)
        {
            serverUIManager.ShowRollButton();
        }
        else if (splineKnotAnimator.inJunction)
        {
            serverUIManager.ShowJunctionButtons();
        }
        else
        {
            serverUIManager.HideAllButtons();
        }
    }

    private void HandleMovementStart(bool moving)
    {
        if (moving)
            serverUIManager.HideAllButtons();
    }
}
