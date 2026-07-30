using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverCanvas;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    [Header("UI & Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    private bool isGameOver = false;

    private void Awake()
    {
        // Simple Singleton to easy access from other scripts
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowPlayScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Player");
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.SetText("Score: {0}", score);
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!!");

        if (playerController != null)
        {
            playerController.DisableMovement();
        }

        Time.timeScale = 0f;

        gameOverCanvas.SetActive(true);
    }
}
