using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class ServerPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private SplineKnotAnimate splineKnotAnimator;
    [SerializeField] private ServerUIManager serverUIManager;
    [SerializeField] private TurnManager turnManager;

    private SplineKnotInstantiate splineKnotData;
    private int index = 0;
    private int currentClient = 1;

    public bool isReady = false;
    public bool asRolled = false;

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

        splineKnotData = FindAnyObjectByType<SplineKnotInstantiate>();
        index = turnManager.players.IndexOf(this); 

        if (index == -1)
        {
            Debug.LogError("This player is not registered in TurnManager!");
        }
    }

    public void ConfirmTurn()
    {
        if (!IsMyTurn()) return;

        isReady = true;
    }

    public void RollDice()
    {
        if (!IsMyTurn()) return;

        asRolled = true;
        int randomNumber = 1;
        Debug.Log($"Player {index} rolled: {randomNumber}");
        splineKnotAnimator.Animate(randomNumber);
    }

    public void ChangeJunction(int direction)
    {
        if (!IsMyTurn()) return;
        splineKnotAnimator.AddToJunctionIndex(direction);
    }

    public void ConfirmJuction()
    {
        if (!IsMyTurn()) return;
        splineKnotAnimator.inJunction = false;
    }

    private void OnDestinationKnot(SplineKnotIndex index)
    {
        var data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];
        if (data.skipStepCount)
            splineKnotAnimator.SkipStepCount = true;
    }

    private void OnKnotLand(SplineKnotIndex index)
    {
        var data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];

        StartCoroutine(DelayCoroutine());
        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(0.08f);
            data.Land(stats);
            OnMovementStart.Invoke(false);

            if (data.spaceEvent == null)
            { 
                EndTurn();
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    private void OnKnotEnter(SplineKnotIndex index)
    {
        var data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];
        data.EnterKnot(splineKnotAnimator, stats);
        OnMovementUpdate.Invoke(splineKnotAnimator.Step);
    }

    public void ContinueMovement()
    {
        Debug.Log("Anda");
        splineKnotAnimator.Paused = false;
    }

    void Update()
    {
        currentClient = turnManager.currentPlayerIndex + 1;
        if (!IsMyTurn()) return;

        
        if (!isReady)
        {
            serverUIManager.ShowReadyMenu((ulong)currentClient);
        }
        else if (!splineKnotAnimator.isMoving && !splineKnotAnimator.inJunction && !asRolled)
        {
            serverUIManager.ShowClientMenu((ulong)currentClient);
        }
        else if (splineKnotAnimator.inJunction)
        {
            serverUIManager.ShowJunctionButtons((ulong)currentClient);
        }
        else
        {
            serverUIManager.HideAllButtons((ulong)currentClient);
        }
    }

    private bool IsMyTurn()
    {
        return turnManager.CurrentPlayer == this;
    }

    public void EndTurn()
    {
        serverUIManager.HideAllButtons((ulong)currentClient);
        turnManager.EndPlayerTurn();
    }
}
