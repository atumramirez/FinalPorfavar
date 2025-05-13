using UnityEngine;
using Unity.Netcode;

public class JumpableObject : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void JumpOneTime()
    {
        if (rb != null)
        {
            Debug.Log("JumpServerRpc called");
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}