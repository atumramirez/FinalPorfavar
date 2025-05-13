using Unity.Netcode;
using UnityEngine;

public class InputSenderlOCal : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Press");
            SendJumpRequest();
        }
    }

    private void SendJumpRequest()
    {
        GameObject jumpObj = GameObject.FindWithTag("JumpTarget");
    }
}