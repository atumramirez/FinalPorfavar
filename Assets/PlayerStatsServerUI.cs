using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsServerUI : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStats PlayerStats;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI healthPoints;
    [SerializeField] private TextMeshProUGUI pointPoints;

    public void UpdateHealthPoints(int points)
    {
        healthPoints.text = "Vida: " + points;
    }

    public void UpdatePointPoints(int points)
    {
        pointPoints.text = "Pontos: " + points;
    }

    public void UpdateHealthAndPointPoints(int healthPoint, int pointPoint)
    {
        healthPoints.text = "Vida: " + healthPoint;
        pointPoints.text = "Pontos: " + pointPoint;
    }
}
