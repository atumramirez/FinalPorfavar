using Unity.Netcode;
using UnityEngine;

public class ShootDice : NetworkBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 vector3)
    {
        rb.AddForce(0, 0, 5, ForceMode.Impulse);
    }
}