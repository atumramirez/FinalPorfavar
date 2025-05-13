using UnityEngine;
using Unity.Netcode;

public class SwipeDirectionDetector : NetworkBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float swipeThreshold = 50f; 

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    DetectSwipe();
                    break;
            }
        }
    }

    void DetectSwipe()
    {
        Vector2 swipe = endTouchPosition - startTouchPosition;

        if (swipe.magnitude < swipeThreshold)
            return; 

        swipe.Normalize();

        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            if (swipe.x > 0)
            {
                SendJumpRequestServerRpc(Vector3.right);
            }
            else
            {
                SendJumpRequestServerRpc(Vector3.left);
            }
        }
        else
        {
            if (swipe.y > 0)
            {
                SendJumpRequestServerRpc(Vector3.up);
            }
            else
            {
                SendJumpRequestServerRpc(Vector3.down);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendJumpRequestServerRpc(Vector3 vector3, ServerRpcParams rpcParams = default)
    {
        GameObject jumpObj = GameObject.FindWithTag("JumpTarget");
        Debug.Log("Procurar objeto.");
        if (jumpObj != null && jumpObj.TryGetComponent(out JumpController controller))
        {
            Debug.Log("Objeto encontrado.");
            controller.Jump(vector3);
        }
    }
}