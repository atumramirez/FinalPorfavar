using UnityEngine;
using System.Collections.Generic;

public class FingerPainter : MonoBehaviour
{
    public Camera cam;
    public GameObject linePrefab;

    private LineRenderer currentLine;
    private List<Vector3> points = new List<Vector3>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.1f)); // Use small positive Z

            if (touch.phase == TouchPhase.Began)
            {
                StartNewLine(touchPos);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                UpdateLine(touchPos);
            }
        }
    }

    void StartNewLine(Vector3 startPos)
    {
        GameObject newLine = Instantiate(linePrefab);
        currentLine = newLine.GetComponent<LineRenderer>();
        points.Clear();
        points.Add(startPos);
        points.Add(startPos);
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, startPos);
        currentLine.SetPosition(1, startPos);
    }

    void UpdateLine(Vector3 newPoint)
    {
        if (Vector3.Distance(points[points.Count - 1], newPoint) > 0.05f)
        {
            points.Add(newPoint);
            currentLine.positionCount = points.Count;
            currentLine.SetPosition(points.Count - 1, newPoint);
        }
    }
}