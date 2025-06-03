using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private SplineKnotAnimate splineKnotAnimator;
    [SerializeField] private ServerUIManager serverUIManager;
    [SerializeField] private TurnManager turnManager;

    private SplineKnotInstantiate splineKnotData;
    private int index = 0;
    private int currentClient = 1;

    private SplineKnotData currentSpace;
    public bool isReady = false;
    public bool asRolled = false;
    public bool asUsedItem = false;
    public int usedItemId;

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
        asUsedItem = false;
    }

    public void RollDice()
    {
        if (!IsMyTurn()) return;

        asRolled = true;
        int randomNumber = 1;
        int finalRoll = randomNumber;


        if (asUsedItem)
        {
            switch (usedItemId)
            {
                case 0:
                    finalRoll += 3;
                    Debug.Log($"Player {index} rolled: {randomNumber} + 3");
                    break;
                case 1:
                    finalRoll *= 2;
                    Debug.Log($"LocoLoco");
                    break;
            }
        }
        else
        {
            Debug.Log($"Player {index} rolled: {randomNumber}");
        }
        splineKnotAnimator.Animate(finalRoll);
    }

    public void GoBackwards()
    {
        Debug.Log("Para atras");
        ContinueMovement();
        splineKnotAnimator.MoveBackward(1);
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
        currentSpace = data;

        StartCoroutine(DelayCoroutine());
        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(0.08f);
            data.Land(splineKnotAnimator);
            OnMovementStart.Invoke(false);

            yield return new WaitForSeconds(1.5f);
        }
    }
    private void OnKnotEnter(SplineKnotIndex index)
    {
        var data = splineKnotData.splineDatas[index.Spline].knots[index.Knot];
        currentSpace = data;
        data.EnterKnot(splineKnotAnimator);
        OnMovementUpdate.Invoke(splineKnotAnimator.Step);
    }

    /// <summary>
    /// EVENTOS:
    /// </summary>
    public void BuyTrap()
    {
        Debug.Log("Quase comprada");
        currentSpace.TryGetComponent(out TrapSpace trapSpaceData);
        trapSpaceData.PlaceTrap(this);
    }

    public void BuyItem(int id)
    {
        stats.BuyItem(id);
        ContinueMovement();
    }

    public void PayWall()
    {
        currentSpace.TryGetComponent(out WallSpace wallSpaceData);
        wallSpaceData.PayCrazy(this);
    }

    public void Teleport()
    {
        currentSpace.TryGetComponent(out StationSpace stationSpaceData);
        splineKnotAnimator.TeleportToKnot(stationSpaceData.splineIndex, stationSpaceData.knotIndex);
        EndTurn();
    }

    public void UseItem(int index)
    {
        asUsedItem = true;
        usedItemId = stats.inventory[index].Id; 
        stats.RemoveItem(index);
        Debug.Log($"Usaste: {usedItemId}");
    }

    public void ContinueMovement()
    {
        Debug.Log("Anda");
        splineKnotAnimator.Paused = false;
    }

    public void StopMovement()
    {
        Debug.Log("Anda");
        splineKnotAnimator.Paused = true;
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
