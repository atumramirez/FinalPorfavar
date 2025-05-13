using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopPromptUI : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void ShowPrompt(Action onYes, Action onNo)
    {
        gameObject.SetActive(true);

        yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            Destroy(gameObject);
        });

        noButton.onClick.AddListener(() =>
        {
            onNo?.Invoke();
            Destroy(gameObject);
        });
    }
}