using UnityEngine;

public class MovingPipe : MonoBehaviour
{
    [SerializeField] private float _speed = 1.30f;

    private void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.left;
    }
}
