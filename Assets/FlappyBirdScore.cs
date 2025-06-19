using UnityEngine;
using TMPro;

public class FlappyBirdScore : MonoBehaviour
{
    public static FlappyBirdScore Instance;

    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _score;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        _scoreText.text = _score.ToString();
    }

    public void UpdateScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
