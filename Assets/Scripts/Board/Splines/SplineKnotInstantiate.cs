using UnityEngine;
using UnityEngine.Splines;
using UnityEditor;
using System.Collections.Generic;

[ExecuteAlways]
public class SplineKnotInstantiate : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private GameObject prefabToInstantiate;

    [Header("Parameters")]
    public Vector3 positionOffset;

    [Header("Data")]
    public List<SplineData> splineDatas = new();

    private void OnEnable()
    {
        if (splineContainer == null)
            splineContainer = GetComponent<SplineContainer>();

        Spline.Changed += OnSplineChanged;
        UpdateSplineData();
    }

    private void OnDisable()
    {
        Spline.Changed -= OnSplineChanged;
    }

    private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
    {
        UpdateSplineData();
        UpdateKnotPositions();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (splineContainer == null)
                splineContainer = GetComponent<SplineContainer>();

            UpdateSplineData();
            UpdateKnotPositions();
        }
    }

    private void UpdateSplineData()
    {
        if (splineContainer == null || prefabToInstantiate == null)
            return;


        Dictionary<(int, int), SplineKnotData> existingKnotData = new Dictionary<(int, int), SplineKnotData>();
        foreach (var splineData in splineDatas)
        {
            for (int i = 0; i < splineData.knots.Count; i++)
            {
                if (splineData.knots[i] != null && splineData.knots[i].gameObject != null)
                {
                    existingKnotData.Add((splineDatas.IndexOf(splineData), i), splineData.knots[i]);
                }
            }
        }


        splineDatas.Clear();

        for (int splineIndex = 0; splineIndex < splineContainer.Splines.Count; splineIndex++)
        {
            var spline = splineContainer.Splines[splineIndex];
            var splineData = new SplineData { knots = new List<SplineKnotData>() };

            for (int knotIndex = 0; knotIndex < spline.Count; knotIndex++)
            {

                var splineKnotIndex = new SplineKnotIndex(splineIndex, knotIndex);
                if (splineContainer.KnotLinkCollection.TryGetKnotLinks(splineKnotIndex, out var connectedKnots))
                {

                    var originalKnot = (splineIndex, knotIndex);
                    foreach (var linkedKnot in connectedKnots)
                    {
                        if (linkedKnot.Spline < originalKnot.Item1 ||
                            (linkedKnot.Spline == originalKnot.Item1 && linkedKnot.Knot < originalKnot.Item2))
                        {
                            originalKnot = (linkedKnot.Spline, linkedKnot.Knot);
                        }
                    }

                    if (existingKnotData.TryGetValue(originalKnot, out var originalKnotData))
                    {
                        splineData.knots.Add(originalKnotData);
                        continue;
                    }
                }


                if (existingKnotData.TryGetValue((splineIndex, knotIndex), out var existingKnot))
                {
                    splineData.knots.Add(existingKnot);
                }
                else
                {
                    InstantiateNewKnot(spline[knotIndex], splineIndex, knotIndex, splineData);
                }
            }

            splineDatas.Add(splineData);
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null) return;
                CleanupUnusedKnots();
            };
        }
        else
#endif
        {
            CleanupUnusedKnots();
        }
    }

    private void CleanupUnusedKnots()
    {
        var knotsToDelete = new List<SplineKnotData>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<SplineKnotData>(out SplineKnotData leftOverData))
            {
                bool isUsed = false;
                foreach (SplineData splineData in splineDatas)
                {
                    if (splineData.knots.Contains(leftOverData))
                    {
                        isUsed = true;
                        break;
                    }
                }

                if (!isUsed)
                {
                    knotsToDelete.Add(leftOverData);
                }
            }
        }

        foreach (var knotToDelete in knotsToDelete)
        {
            if (knotToDelete != null && knotToDelete.gameObject != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(knotToDelete.gameObject);
                else
#endif
                    Destroy(knotToDelete.gameObject);
            }
        }
    }


    private void InstantiateNewKnot(BezierKnot knot, int splineIndex, int knotIndex, SplineData splineData)
    {
        GameObject instantiatedObject;
#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfPrefabAsset(prefabToInstantiate))
        {
            instantiatedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabToInstantiate, transform);
        }
        else
        {
            instantiatedObject = Instantiate(prefabToInstantiate, transform);
        }
#else
        instantiatedObject = Instantiate(prefabToInstantiate, transform);
#endif

        instantiatedObject.name = $"S{splineIndex}K{knotIndex}";
        instantiatedObject.transform.position = (Vector3)knot.Position + splineContainer.transform.position;
        instantiatedObject.transform.rotation = knot.Rotation;

        if (instantiatedObject.TryGetComponent<SplineKnotData>(out SplineKnotData data))
        {
            data.knotIndex = new SplineKnotIndex(splineIndex, knotIndex);
            splineData.knots.Add(data);
        }
        else
        {
            Debug.LogError("The instantiated prefab does not have a SplineKnotData component!");
            return;
        }
    }

    private void UpdateKnotPositions()
    {
        try
        {
            for (int i = 0; i < splineDatas.Count; i++)
            {
                if (i >= splineContainer.Splines.Count)
                {
                    UpdateSplineData(); 
                    return;
                }

                var spline = splineContainer.Splines[i];

                for (int j = 0; j < splineDatas[i].knots.Count; j++)
                {
                    if (j >= spline.Count)
                    {
                        UpdateSplineData();
                        return;
                    }

                    var knot = spline[j];
                    var knotData = splineDatas[i].knots[j];

                    if (knotData != null && knotData.gameObject != null)
                    {
                        knotData.gameObject.transform.position = (Vector3)knot.Position + splineContainer.transform.position + positionOffset;
                        knotData.gameObject.transform.rotation = knot.Rotation;
                    }
                }
            }
        }
        catch (System.ArgumentOutOfRangeException)
        {
            UpdateSplineData();
        }
    }

    private void OnDestroy()
    {
        ResetDataAndObjects();
    }

    [ContextMenu("Delete Data")]
    public void ResetDataAndObjects()
    {
        foreach (SplineData splineData in splineDatas)
        {
            foreach (SplineKnotData knotData in splineData.knots)
            {
                if (knotData != null && knotData.gameObject != null)
                {
                    DestroyImmediate(knotData.gameObject);
                }
            }
        }
        splineDatas.Clear();
    }

    [ContextMenu("Clear and Populate")]
    public void ResetAndPopulate()
    {
        ResetDataAndObjects();
        UpdateSplineData();
        UpdateKnotPositions();
    }

    [System.Serializable]
    public class SplineData
    {
        public List<SplineKnotData> knots;
    }
}
