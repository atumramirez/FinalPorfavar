using UnityEngine;

public class RollMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector3 tilt = Input.acceleration;
        Vector3 move = new Vector3(tilt.x, 0, tilt.y);
        transform.Translate(move * speed * Time.deltaTime);
    }

}
