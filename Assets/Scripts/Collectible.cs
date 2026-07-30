using UnityEngine;
using TMPro;

public class Collectible : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Sprite[] gemSprites;

    private static int score;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite();

        // Permite asignar el texto desde la jerarquía y, si se omite, busca el
        // objeto de UI existente llamado "ScoreText".
        if (scoreText == null)
        {
            GameObject scoreTextObject = GameObject.Find("ScoreText");
            scoreText = scoreTextObject != null
                ? scoreTextObject.GetComponent<TextMeshProUGUI>()
                : null;
        }

        if (scoreText == null)
        {
            Debug.LogError("No se encontró un TextMeshProUGUI para el marcador.", this);
            return;
        }

        scoreText.SetText("Score: {0}", score);
    }

    private void SetRandomSprite()
    {
        if (spriteRenderer == null || gemSprites == null || gemSprites.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, gemSprites.Length);
        spriteRenderer.sprite = gemSprites[randomIndex];
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            score++;
            scoreText.SetText("Score: {0}", score);
            Destroy(gameObject);
        }
    }
}
