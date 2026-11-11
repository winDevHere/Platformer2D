using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int startingLives = 3;
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winPanel;            // NEW

    int lives;
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        lives = startingLives;
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);                   // NEW
        UpdateLivesText();
    }

    public bool LoseLife()
    {
        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            GameOver();
            return true;
        }
        return false;
    }

    void GameOver()
    {
        IsGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void WinLevel()                           // NEW
    {
        if (IsGameOver) return;
        IsGameOver = true;                           // also stops enemies/falling from costing lives
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateLivesText() => livesText.text = "Lives: " + lives;
}