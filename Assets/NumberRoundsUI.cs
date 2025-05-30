using UnityEngine;
using TMPro;
   
public class NumberRoundsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    private int totalRounds;

    private void Start()
    {
        roundText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void UpdateText(int round)
    {
        roundText.text = $"{round}/{totalRounds}";
    }

    public void GetTotalRounds(int rounds)
    {
        totalRounds = rounds;  
    }
}
