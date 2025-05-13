using Unity.Netcode;
using UnityEngine;

public class ServerJumpObject : NetworkBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;

    // NetworkVariable to track jump requests
    public NetworkVariable<bool> JumpRequested = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Subscribe to value changes
        if (IsServer)
        {
            JumpRequested.OnValueChanged += OnJumpRequestedChanged;
        }
    }

    private void OnJumpRequestedChanged(bool previous, bool current)
    {
        if (current && IsServer)
        {
            PerformJump();
            // Reset the value immediately after jumping
            JumpRequested.Value = false;
        }
    }

    private void PerformJump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            JumpRequested.OnValueChanged -= OnJumpRequestedChanged;
        }
        base.OnNetworkDespawn();
    }
}