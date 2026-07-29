using UnityEngine;
using TMPro;

public class Collectible : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private static int score;

    private void Awake()
    {
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
