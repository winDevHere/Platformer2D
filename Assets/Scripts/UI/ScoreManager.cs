using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Persists across scene loads so the coin count carries from level to level.
// Every scene still keeps its own "Score Manager" object wired to that scene's
// score text: when a new scene loads, its copy hands the text over to the
// surviving instance and destroys itself.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TMP_Text scoreText;

    int score;
    int scoreAtLevelStart;   // restored when the same level is reloaded (restart / game over)
    int currentLevel = -1;

    void Awake()
    {
        int level = SceneManager.GetActiveScene().buildIndex;

        if (Instance != null && Instance != this)
        {
            Instance.EnterLevel(level, scoreText);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);   // DontDestroyOnLoad only works on root objects
        DontDestroyOnLoad(gameObject);
        EnterLevel(level, scoreText);
    }

    void EnterLevel(int level, TMP_Text text)
    {
        if (level == currentLevel)
            score = scoreAtLevelStart;   // reloading the same level: drop coins picked up on the failed attempt
        else
            scoreAtLevelStart = score;   // new level: bank the score so far

        currentLevel = level;
        scoreText = text;
        UpdateText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateText();
    }

    void UpdateText()
    {
        if (scoreText != null) scoreText.text = "Coins: " + score;
    }
}
