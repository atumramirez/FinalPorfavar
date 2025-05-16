using UnityEngine;

public class TouchMapController : MonoBehaviour
{
    public float panSpeed = 0.5f;
    public float zoomSpeed = 0.01f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            // Pan
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 delta = new Vector3(-touch.deltaPosition.x * panSpeed * Time.deltaTime, -touch.deltaPosition.y * panSpeed * Time.deltaTime, 0);
                cam.transform.Translate(delta);
            }
        }
        else if (Input.touchCount == 2)
        {
            // Zoom
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}