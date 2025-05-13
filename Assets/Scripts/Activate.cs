using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectToggler : NetworkBehaviour
{
    [Header("Set objects to activate/deactivate on CLIENTS")]
    public List<GameObject> clientObjs;
    public List<GameObject> serverObjs;

    public override void OnNetworkSpawn()
    {
        // Run server-side toggle (includes host)
        if (IsServer)
        {
            ToggleObjects(serverObjs, clientObjs);
        }

        // Run client-side toggle (excluding host)
        if (IsClient && !IsServer)
        {
            ToggleObjects(clientObjs, serverObjs);
        }
    }

    private void ToggleObjects(List<GameObject> toActivate, List<GameObject> toDeactivate)
    {
        foreach (var obj in toActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (var obj in toDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}