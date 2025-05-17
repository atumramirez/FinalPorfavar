using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiceMover : MonoBehaviour
{
    public float force = 5f;
    public float torque = 10f;
    private Rigidbody rb;
    private bool isStopping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Roll()
    {
        // Force strictly along Z axis
        Vector3 dir = Vector3.forward;
        rb.AddForce(dir * force, ForceMode.Impulse);

        // Random tumbling torque
        rb.AddTorque(Random.onUnitSphere * torque, ForceMode.Impulse);

        // Auto-stop after ~1 second
        Invoke(nameof(SnapToFace), 1f);
    }

    private void SnapToFace()
    {
        if (isStopping) return;
        isStopping = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.rotation = GetClosestUprightRotation();
    }

    private Quaternion GetClosestUprightRotation()
    {
        Quaternion[] uprightRotations = new Quaternion[]
        {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, 270, 0),
            Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(-90, 0, 0),
        };

        Quaternion closest = uprightRotations[0];
        float minAngle = Quaternion.Angle(transform.rotation, closest);

        foreach (var rot in uprightRotations)
        {
            float angle = Quaternion.Angle(transform.rotation, rot);
            if (angle < minAngle)
            {
                minAngle = angle;
                closest = rot;
            }
        }

        return closest;
    }
}
