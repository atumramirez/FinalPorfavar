using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class ShapeDrawer : MonoBehaviour
{
    public float pointSpacing = 0.1f;
    public float closeThreshold = 0.5f;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private bool isDrawing = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            points.Clear();
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            if (points.Count == 0 || Vector3.Distance(points.Last(), mouseWorld) > pointSpacing)
            {
                points.Add(mouseWorld);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
            }
        }

        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;

            if (Vector3.Distance(points[0], points.Last()) < closeThreshold)
            {
                DetectShape(points);
            }
            else
            {
                Debug.Log("Shape not closed.");
            }
        }
    }

    void DetectShape(List<Vector3> points)
    {
        // Simplify the line by reducing points
        List<Vector3> simplified = RamerDouglasPeucker(points, 0.1f);

        int cornerCount = simplified.Count;

        // Remove closing duplicate point if present
        if (Vector3.Distance(simplified[0], simplified.Last()) < 0.1f)
            simplified.RemoveAt(simplified.Count - 1);

        float perimeter = 0f;
        for (int i = 0; i < simplified.Count; i++)
        {
            perimeter += Vector3.Distance(simplified[i], simplified[(i + 1) % simplified.Count]);
        }

        float area = PolygonArea(simplified);
        float circularity = 4 * Mathf.PI * area / (perimeter * perimeter);

        string detected = "Unknown";

        if (cornerCount == 3)
            detected = "Triangle";
        else if (cornerCount == 4)
            detected = "Square";
        else if (circularity > 0.7f)
            detected = "Circle";

        Debug.Log("Detected:" + detected);
    }

    // Estimate polygon area
    float PolygonArea(List<Vector3> verts)
    {
        float area = 0;
        for (int i = 0; i < verts.Count; i++)
        {
            Vector3 p1 = verts[i];
            Vector3 p2 = verts[(i + 1) % verts.Count];
            area += (p1.x * p2.y) - (p2.x * p1.y);
        }
        return Mathf.Abs(area) / 2f;
    }

    // Ramer–Douglas–Peucker algorithm for point simplification
    List<Vector3> RamerDouglasPeucker(List<Vector3> points, float epsilon)
    {
        if (points.Count < 3)
            return new List<Vector3>(points);

        int index = -1;
        float maxDistance = 0f;

        for (int i = 1; i < points.Count - 1; i++)
        {
            float dist = PerpendicularDistance(points[i], points[0], points.Last());
            if (dist > maxDistance)
            {
                index = i;
                maxDistance = dist;
            }
        }

        if (maxDistance > epsilon)
        {
            var left = RamerDouglasPeucker(points.GetRange(0, index + 1), epsilon);
            var right = RamerDouglasPeucker(points.GetRange(index, points.Count - index), epsilon);
            left.RemoveAt(left.Count - 1);
            left.AddRange(right);
            return left;
        }
        else
        {
            return new List<Vector3> { points[0], points.Last() };
        }
    }

    float PerpendicularDistance(Vector3 pt, Vector3 lineStart, Vector3 lineEnd)
    {
        if (lineStart == lineEnd)
            return Vector3.Distance(pt, lineStart);

        Vector3 dir = lineEnd - lineStart;
        Vector3 proj = Vector3.Project(pt - lineStart, dir.normalized);
        Vector3 closest = lineStart + proj;
        return Vector3.Distance(pt, closest);
    }
}