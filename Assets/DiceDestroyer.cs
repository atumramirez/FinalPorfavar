using UnityEngine;

public class DiceDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dice"))
        {
            Destroy(collision.gameObject);
        }
    }
}