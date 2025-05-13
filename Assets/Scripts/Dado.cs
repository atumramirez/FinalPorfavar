using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableDice : MonoBehaviour
{
    private Rigidbody rb;
    private Camera mainCamera;

    private bool isHeld = false;
    private Vector3 lastMouseWorldPos;
    private Vector3 throwVelocity;

    public float followSpeed = 10f;
    public float throwForceMultiplier = 10f;
    private float fixedY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (isHeld)
        {
            // Continuous rotation while being held
            transform.Rotate(new Vector3(90, 120, 60) * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        rb.isKinematic = true;
        isHeld = true;
    }

    void OnMouseDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPoint = ray.GetPoint(enter);
            Vector3 targetPos = new Vector3(targetPoint.x, fixedY, targetPoint.z);

            throwVelocity = (targetPos - lastMouseWorldPos) * throwForceMultiplier;
            lastMouseWorldPos = targetPos;

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }

    void OnMouseUp()
    {
        isHeld = false;
        rb.isKinematic = false;

        rb.linearVelocity = new Vector3(throwVelocity.x, 0, throwVelocity.z);
        rb.AddTorque(Random.onUnitSphere * throwVelocity.magnitude, ForceMode.Impulse);
    }
}