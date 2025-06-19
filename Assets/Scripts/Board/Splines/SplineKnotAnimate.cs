using UnityEngine;
using UnityEngine.Splines;
using System.Linq;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class SplineKnotAnimate : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float movementLerp = 10;
    [SerializeField] private float rotationLerp = 10;
    public int remainingSteps;
    public int Step => remainingSteps;

    [Header("Knot Logic")]
    public SplineKnotIndex currentKnot;
    public SplineKnotIndex nextKnot;
    private IReadOnlyList<SplineKnotIndex> connectedKnots;

    [Header("Interpolation")]
    private float currentT;

    [Header("Junction Parameters")]
    public int junctionIndex = 0;
    public List<SplineKnotIndex> walkableKnots = new List<SplineKnotIndex>();

    [Header("States")]
    public bool isMoving = false;
    public bool inJunction = false;
    public bool Paused = false;
    [HideInInspector] public bool SkipStepCount = false;

    [Header("Current State")]
    public SplineKnotIndex currentSpace;

    [Header("Events")]
    [HideInInspector] public UnityEvent<bool> OnEnterJunction;
    [HideInInspector] public UnityEvent<int> OnJunctionSelection;
    [HideInInspector] public UnityEvent<SplineKnotIndex> OnDestinationKnot;
    [HideInInspector] public UnityEvent<SplineKnotIndex> OnKnotEnter;
    [HideInInspector] public UnityEvent<SplineKnotIndex> OnKnotLand;

    void Start()
    {
        if (splineContainer == null)
        {
            Debug.LogError("Spline Container not assigned!");
            return;
        }

        currentKnot.Knot = 0;
        currentKnot.Spline = 0;
        currentT = 0;
        Spline spline = splineContainer.Splines[currentKnot.Spline];
        nextKnot = new SplineKnotIndex(currentKnot.Spline, (currentKnot.Knot + 1) % spline.Knots.Count());
    }

    private void Update()
    {
        MoveAndRotate();
    }

    public void Animate(int stepAmount = 1)
    {
        if (isMoving)
        {
            Debug.Log("Already animating");
            return;
        }

        remainingSteps = stepAmount;

        currentSpace = currentKnot;

        StartCoroutine(MoveAlongSpline());
    }

    public void MoveBackward(int stepAmount = 1)
    {
        if (!Paused || isMoving)
        {
            Debug.LogWarning("MoveBackward called in invalid state: must be paused and not already moving.");
            return;
        }

        Paused = false;
        currentSpace = currentKnot;

        StartCoroutine(MoveBackwardOneStepThenForward(stepAmount));
    }

    public void TeleportToKnot(int splineIndex, int knotIndex)
    {
        if (splineIndex < 0 || splineIndex >= splineContainer.Splines.Count)
        {
            Debug.LogWarning("Invalid spline index.");
            return;
        }

        var spline = splineContainer.Splines[splineIndex];

        if (knotIndex < 0 || knotIndex >= spline.Knots.Count())
        {
            Debug.LogWarning("Invalid knot index.");
            return;
        }

        currentKnot = new SplineKnotIndex(splineIndex, knotIndex);

        isMoving = false;
        inJunction = false;
        Paused = false;
        remainingSteps = 0;
        walkableKnots.Clear();

        currentT = spline.ConvertIndexUnit(knotIndex, PathIndexUnit.Knot, PathIndexUnit.Normalized);
        nextKnot = new SplineKnotIndex(splineIndex, (knotIndex + 1) % spline.Knots.Count());

        Vector3 worldPos = (Vector3)spline.EvaluatePosition(currentT) + splineContainer.transform.position;
        transform.position = worldPos;

        spline.Evaluate(currentT, out float3 pos, out float3 dir, out float3 up);
        Vector3 worldDirection = splineContainer.transform.TransformDirection(dir);
        if (worldDirection.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(worldDirection, Vector3.up);

        currentSpace = currentKnot;

        Debug.Log($"Teleported to Spline {splineIndex}, Knot {knotIndex}");
    }

    IEnumerator MoveAlongSpline()
    {
        if (inJunction)
        {
            yield return new WaitUntil(() => inJunction == false);
            OnEnterJunction.Invoke(false);
            SelectJunctionPath(junctionIndex);
        }

        if (Paused)
            yield return new WaitUntil(() => Paused == false);

        isMoving = true;

        Spline spline = splineContainer.Splines[currentKnot.Spline];
        nextKnot = new SplineKnotIndex(currentKnot.Spline, (currentKnot.Knot + 1) % spline.Knots.Count());
        currentT = spline.ConvertIndexUnit(currentKnot.Knot, PathIndexUnit.Knot, PathIndexUnit.Normalized);
        float nextT;

        OnDestinationKnot.Invoke(nextKnot);

        if (nextKnot.Knot == 0 && spline.Closed)
            nextT = 1f;
        else
            nextT = spline.ConvertIndexUnit(nextKnot.Knot, PathIndexUnit.Knot, PathIndexUnit.Normalized);

        while (currentT != nextT)
        {
            currentT = Mathf.MoveTowards(currentT, nextT, AdjustedMovementSpeed(spline) * Time.deltaTime);
            yield return null;
        }

        if (currentT >= nextT)
        {
            currentKnot = nextKnot;
            nextKnot = new SplineKnotIndex(currentKnot.Spline, (currentKnot.Knot + 1) % spline.Knots.Count());

            if (nextT == 1)
                currentT = 0;

            splineContainer.KnotLinkCollection.TryGetKnotLinks(currentKnot, out connectedKnots);

            if (IsJunctionKnot(currentKnot))
            {
                inJunction = true;
                junctionIndex = 0;
                isMoving = false;
                OnEnterJunction.Invoke(true);
                OnJunctionSelection.Invoke(junctionIndex);
            }
            else
            {
                if (!SkipStepCount)
                    remainingSteps--;
                else
                    SkipStepCount = false;
            }

            OnKnotEnter.Invoke(currentKnot);

            if (IsLastKnot(currentKnot) && connectedKnots != null)
            {
                foreach (SplineKnotIndex connKnot in connectedKnots)
                {
                    if (!IsLastKnot(connKnot))
                    {
                        currentKnot = connKnot;
                        currentT = splineContainer.Splines[currentKnot.Spline].ConvertIndexUnit(connKnot.Knot, PathIndexUnit.Knot, PathIndexUnit.Normalized);
                    }
                }
            }

            if (remainingSteps > 0)
            {
                StartCoroutine(MoveAlongSpline());
            }
            else
            {
                isMoving = false;
                OnKnotLand.Invoke(currentKnot);


                currentSpace = currentKnot;
                Debug.Log($"Stopped at Spline {currentKnot.Spline}, Knot {currentKnot.Knot}");
            }
        }
    }

    IEnumerator MoveBackwardOneStepThenForward(int originalSteps)
    {
        isMoving = true;

        // Step 1: move one space backward
        Spline spline = splineContainer.Splines[currentKnot.Spline];
        int prevKnotIndex = (currentKnot.Knot - 1 + spline.Knots.Count()) % spline.Knots.Count();
        nextKnot = new SplineKnotIndex(currentKnot.Spline, prevKnotIndex);

        currentT = spline.ConvertIndexUnit(currentKnot.Knot, PathIndexUnit.Knot, PathIndexUnit.Normalized);
        float nextT = spline.ConvertIndexUnit(nextKnot.Knot, PathIndexUnit.Knot, PathIndexUnit.Normalized);

        OnDestinationKnot.Invoke(nextKnot);

        while (currentT != nextT)
        {
            currentT = Mathf.MoveTowards(currentT, nextT, AdjustedMovementSpeed(spline) * Time.deltaTime);
            yield return null;
        }

        // Finished backward movement
        currentKnot = nextKnot;
        currentSpace = currentKnot;
        splineContainer.KnotLinkCollection.TryGetKnotLinks(currentKnot, out connectedKnots);

        OnKnotEnter.Invoke(currentKnot);
        OnKnotLand.Invoke(currentKnot);

        Debug.Log($"Moved backward to Spline {currentKnot.Spline}, Knot {currentKnot.Knot}");

        // Step 2: continue moving forward for original steps
        remainingSteps = originalSteps;
        StartCoroutine(MoveAlongSpline());
    }

    void MoveAndRotate()
    {
        float movementBlend = Mathf.Pow(0.5f, Time.deltaTime * movementLerp);

        Vector3 targetPosition = (Vector3)splineContainer.EvaluatePosition(currentKnot.Spline, currentT);
        transform.position = Vector3.Lerp(targetPosition, transform.position, movementBlend);

        splineContainer.Splines[currentKnot.Spline].Evaluate(currentT, out float3 position, out float3 direction, out float3 up);

        Vector3 worldDirection = splineContainer.transform.TransformDirection(direction);

        if (worldDirection.sqrMagnitude > 0.0001f && isMoving)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(worldDirection, Vector3.up), rotationLerp * Time.deltaTime);
    }

    public void AddToJunctionIndex(int amount)
    {
        if (!inJunction)
            return;
        junctionIndex = (int)Mathf.Repeat(junctionIndex + amount, walkableKnots.Count);
        OnJunctionSelection.Invoke(junctionIndex);
    }

    public void SelectJunctionPath(int index)
    {
        if (walkableKnots.Count < 1)
            return;

        SplineKnotIndex selectedKnot = walkableKnots[index];
        currentKnot = selectedKnot;

        Spline spline = splineContainer.Splines[currentKnot.Spline];
        nextKnot = new SplineKnotIndex(currentKnot.Spline, (currentKnot.Knot + 1) % spline.Knots.Count());

        currentT = splineContainer.Splines[currentKnot.Spline].ConvertIndexUnit(currentKnot.Knot, PathIndexUnit.Knot, PathIndexUnit.Normalized);

        walkableKnots.Clear();
    }

    public Vector3 GetJunctionPathPosition(int index)
    {
        if (walkableKnots.Count < 1)
            return Vector3.zero;

        SplineKnotIndex walkableKnotIndex = walkableKnots[index];
        Spline walkableSpline = splineContainer.Splines[walkableKnotIndex.Spline];
        SplineKnotIndex nextWalkableKnotIndex = new SplineKnotIndex(walkableKnotIndex.Spline, (walkableKnotIndex.Knot + 1) % walkableSpline.Knots.Count());
        Vector3 knotPosition = (Vector3)walkableSpline.Knots.ToArray()[nextWalkableKnotIndex.Knot].Position + splineContainer.transform.position;
        return knotPosition;
    }

    bool IsJunctionKnot(SplineKnotIndex knotIndex)
    {
        walkableKnots.Clear();

        if (connectedKnots == null || connectedKnots.Count == 0)
            return false;

        int divergingPaths = 0;

        foreach (SplineKnotIndex connection in connectedKnots)
        {
            var spline = splineContainer.Splines[connection.Spline];

            if (!IsLastKnot(connection))
            {
                divergingPaths++;
                walkableKnots.Add(connection);
            }
        }

        walkableKnots.Sort((knot1, knot2) => knot1.Spline.CompareTo(knot2.Spline));

        if (divergingPaths <= 1)
            walkableKnots.Clear();

        return divergingPaths > 1;
    }


    bool IsLastKnot(SplineKnotIndex knotIndex)
    {
        var spline = splineContainer.Splines[knotIndex.Spline];
        return knotIndex.Knot >= spline.Knots.ToArray().Length - 1 && !splineContainer.Splines[knotIndex.Spline].Closed;
    }

    bool IsFirstKnot(SplineKnotIndex knotIndex)
    {
        return knotIndex.Knot == 0 && !splineContainer.Splines[knotIndex.Spline].Closed;
    }

    float AdjustedMovementSpeed(Spline spline)
    {

        float splineLength = spline.GetLength();

        return moveSpeed / splineLength;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (inJunction)
            Gizmos.DrawSphere(GetJunctionPathPosition(junctionIndex), 1);
    }
}
