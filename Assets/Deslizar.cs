using UnityEngine;

public class DiceRollController : MonoBehaviour
{
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    [Header("Swipe Settings")]
    public float minSwipeDistance = 100f; // minimum vertical distance for a swipe to count

    [Header("Dice")]
    public int diceResult;
    public int minRoll = 1;
    public int maxRoll = 6;

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();  // Use mouse for testing
#else
        HandleTouchInput();
#endif
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    touchEndPos = touch.position;
                    DetectSwipe();
                    break;
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchEndPos = Input.mousePosition;
            DetectSwipe();
        }
    }

    void DetectSwipe()
    {
        float verticalMove = touchEndPos.y - touchStartPos.y;
        float horizontalMove = Mathf.Abs(touchEndPos.x - touchStartPos.x);

        if (verticalMove > minSwipeDistance && verticalMove > horizontalMove)
        {
            Debug.Log("Deslizaste puta");
        }
    }

    void RollDice()
    {
        diceResult = Random.Range(minRoll, maxRoll + 1);
        Debug.Log("Dice Rolled: " + diceResult);
        // Trigger animation, sound, or UI here
    }
}

