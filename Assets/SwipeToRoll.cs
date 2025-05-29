using UnityEngine;
using System.Collections;


public class SwipeToRoll : MonoBehaviour
{
    [SerializeField] private ClientController controller;
    [SerializeField] private ClientPlayerUIController clientPlayerUIController;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    [Header("Swipe Settings")]
    public float minSwipeDistance = 100f;

    [SerializeField] private Animator diceAnimator;

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();  
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

    private bool isThrowing = false;

    void DetectSwipe()
    {
        if (isThrowing) return; 

        float verticalMove = touchEndPos.y - touchStartPos.y;
        float horizontalMove = Mathf.Abs(touchEndPos.x - touchStartPos.x);

        if (verticalMove > minSwipeDistance && verticalMove > horizontalMove)
        {
            diceAnimator.SetTrigger("Throw");
            StartCoroutine(WaitForAnimation("ThrowDice"));
        }
    }

    IEnumerator WaitForAnimation(string animationName)
    {
        isThrowing = true;

        while (!diceAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        while (diceAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        isThrowing = false;

        Debug.Log("Animation Finished!");
        clientPlayerUIController.RollDice();

    }

    public void Idle()
    {
        diceAnimator.SetTrigger("Idle");
    }

    /// <summary>
    /// Voltar atras
    /// </summary>

    public void VoltarAtras()
    {
        controller.PressBack();
    }
}