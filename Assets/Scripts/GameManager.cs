using TMPro;
using UnityEngine;
using System.Collections;
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

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip collectibleSound;
    [SerializeField] private AudioClip obstacleHitSound;
    [SerializeField] private AudioClip gameOverMusic;

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

        // Tries to get the AudioSource if it wasn't assigned in Inspector
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            audioSource = sources[0];
            sfxSource = sources[1];
        }
        else if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
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

        PlayCollectibleSound();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.SetText("Score: {0}", score);
        }
    }

    private void PlayCollectibleSound()
    {
        if (sfxSource != null && collectibleSound != null)
        {
            sfxSource.PlayOneShot(collectibleSound);
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

        audioSource.Stop();

        if (playerController != null)
        {
            playerController.DisableMovement();
        }

        // Start game over audio sequence
        StartCoroutine(GameOverAudioSequence());

        Time.timeScale = 0f;

        gameOverCanvas.SetActive(true);
    }

    private IEnumerator GameOverAudioSequence()
    {
        float crashDuration = 0f;

        if (sfxSource != null && obstacleHitSound != null)
        {
            sfxSource.PlayOneShot(obstacleHitSound);
            crashDuration = (obstacleHitSound.length - 1f);
        }

        yield return new WaitForSecondsRealtime(crashDuration);

        if (audioSource != null)
        {
            //audioSource.Stop();

            // Change the clip to Game Over theme y play it
            if (gameOverMusic != null)
            {
                audioSource.clip = gameOverMusic;
                audioSource.loop = false;
                audioSource.volume = 0.5f;
                audioSource.Play();
            }
        }
    }
}
