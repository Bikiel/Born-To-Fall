using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverCanvas;
    public bool GameoverFreeze = false;
    [Header("Game Over Settings")]
    public float gameOverDelay = 3f; // Tiempo de espera configurable

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
        SceneManager.LoadScene("PlayerRevCami");
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

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // 1. Desactivar controles del jugador inmediatamente pero dejar correr las físicas/partículas
        if (playerController != null)
        {
            playerController.DisableMovement();
        }

        // 2. Reproducir sonido de choque/impacto
        if (sfxSource != null && obstacleHitSound != null)
        {
            sfxSource.PlayOneShot(obstacleHitSound);
        }

        // 3. Esperar el tiempo configurado para que se vean las partículas y suene el impacto
        yield return new WaitForSeconds(gameOverDelay);

        // 4. Detener música de fondo y cambiar a la música de GameOver
        if (audioSource != null)
        {
            audioSource.Stop();

            if (gameOverMusic != null)
            {
                audioSource.clip = gameOverMusic;
                audioSource.loop = false;
                audioSource.volume = 0.5f;
                audioSource.Play();
            }
        }

        // 5. Pausar la simulación y mostrar la UI


        //Time.timeScale = 0f;
        //pausar la simulación del juego?
        if(GameoverFreeze)
        {
            Time.timeScale = 0f;
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
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
