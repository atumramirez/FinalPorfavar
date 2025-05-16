using UnityEngine;
using Unity.Netcode;

public class Collidering : NetworkBehaviour
{
    [SerializeField] private ClientPlayerUIController clientPlayerUIController;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
