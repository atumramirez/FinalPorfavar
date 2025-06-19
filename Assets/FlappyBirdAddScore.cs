using UnityEngine;

public class FlappyBirdAddScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FlappyBirdScore.Instance.UpdateScore();
        }
    }
}
