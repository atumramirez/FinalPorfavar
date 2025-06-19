using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyBirdGameManager : MonoBehaviour
{
    public static FlappyBirdGameManager Instance;

    [SerializeField] private GameObject _restartMenu;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        _restartMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("SceneManager.GetActiveScene.buildIndex");
    }
}
