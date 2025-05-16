using UnityEngine;

public class SwipeToRoll : MonoBehaviour
{
    [SerializeField] private ClientController controller;
    [SerializeField] private ClientPlayerUIController clientPlayerUIController;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    [Header("Swipe Settings")]
    public float minSwipeDistance = 100f; 

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();  // Use mouse for testing
#else
        HandleTouchInput();
#endif
    }

    /// <summary>
    /// Detetetar o swipe do jogador para lançar os dados
    /// </summary>
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
            clientPlayerUIController.RollDice();
        }
    }

    /// <summary>
    /// Voltar atras
    /// </summary>

    public void VoltarAtras()
    {
        controller.PressBack();
    }
}