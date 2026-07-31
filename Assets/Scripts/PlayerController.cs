using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;

    [Header("Components")]
    public SpriteRenderer playerSprite; // Variable pública para el SpriteRenderer

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float horizontalLimit = 8.4f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2f; // Tiempo que tarda en desaparecer del todo

    private Rigidbody2D playerRigidbody;
    private bool isGameOver;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        if (playerSprite == null)
        {
            playerSprite = GetComponent<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }

        Vector2 position = playerRigidbody.position;
        position.x = Mathf.Clamp(position.x, -horizontalLimit, horizontalLimit);
        playerRigidbody.position = position;

        float horizontalInput = moveAction.ReadValue<Vector2>().x;
        float targetVelocityX = horizontalInput * moveSpeed;
        float velocityX = Mathf.MoveTowards(
            playerRigidbody.linearVelocity.x,
            targetVelocityX,
            acceleration * Time.fixedDeltaTime);

        float nextPositionX = playerRigidbody.position.x + velocityX * Time.fixedDeltaTime;
        if (nextPositionX > horizontalLimit || nextPositionX < -horizontalLimit)
        {
            velocityX = 0f;
        }

        playerRigidbody.linearVelocity = new Vector2(velocityX, playerRigidbody.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
            Destroy(collision.gameObject);
        }
    }

    public void DisableMovement()
    {
        if (isGameOver) return;
        isGameOver = true;

        moveAction.Disable();

        // 1. Descongelar las restricciones del Rigidbody2D inmediatamente (reacciona al choque)
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
        }

        // 2. Iniciar el desvanecimiento progresivo
        if (playerSprite != null)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        Color initialColor = playerSprite.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculamos el alpha de 1 a 0 gradualmente
            float newAlpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / fadeDuration);
            playerSprite.color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);

            yield return null;
        }

        // Aseguramos que quede completamente invisible
        playerSprite.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }
}
