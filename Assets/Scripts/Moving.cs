using UnityEngine;
using Unity.Netcode;

public class ArrowKeyMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (!IsOwner) return;
        float moveX = Input.GetAxisRaw("Horizontal"); // Left/Right arrows
        float moveY = Input.GetAxisRaw("Vertical");   // Up/Down arrows

        Vector3 movement = new Vector3(moveX, moveY, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}