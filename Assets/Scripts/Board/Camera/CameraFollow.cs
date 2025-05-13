using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Player's transform
    public Vector3 offset = new Vector3(0, 5, -10); // Camera offset from player
    public float followSpeed = 5f;   // Smooth speed

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on player + offset
        Vector3 desiredPosition = target.position + offset;

        // Smooth transition from current to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // Optional: keep looking at the player
        transform.LookAt(target);
    }
}