using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TMP_Text scoreText;
    int score;

    void Awake()
    {
        Instance = this;
        UpdateText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateText();
    }

    void UpdateText() => scoreText.text = "Coins: " + score;
}