using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class JumpButtonHandler : NetworkBehaviour
{
    public void PressButton()
    {
        OnJumpButtonPressedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnJumpButtonPressedServerRpc()
    {
        GameObject targetObject = GameObject.Find("TargetObjectName");

        if (targetObject != null)
        {
            JumpableObject script = targetObject.GetComponent<JumpableObject>();

            if (script != null)
            {
                script.JumpOneTime();
            }
            else
            {
                Debug.LogWarning("TargetScript not found on the object.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject not found.");
        }
    }
}