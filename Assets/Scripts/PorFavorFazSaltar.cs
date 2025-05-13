using Unity.Netcode;
using UnityEngine;

public class JumpController : NetworkBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Jump(Vector3 vector3)
    {
        rb.AddForce(vector3 * 5f, ForceMode.Impulse);
    }
}