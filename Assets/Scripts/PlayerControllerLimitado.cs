using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerLimitado : MonoBehaviour
{
    public InputAction moveAction;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 25f;

    [Header("Límites de Movimiento (Desde la pos. inicial)")]
    public Vector2 limiteIzquierdaDerecha = new Vector2(5f, 6f); // X: Izquierda, Y: Derecha
    public Vector2 limiteAbajoArriba = new Vector2(3f, 4f);      // X: Abajo, Y: Arriba

    private Rigidbody2D playerRigidbody;
    private bool isGameOver;

    // Variables internas para almacenar el rango absoluto permitido
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Guardamos la posición original al iniciar
        Vector2 startPosition = transform.position;

        // Calculamos los límites mínimos y máximos globales
        minBounds = new Vector2(startPosition.x - limiteIzquierdaDerecha.x, startPosition.y - limiteAbajoArriba.x);
        maxBounds = new Vector2(startPosition.x + limiteIzquierdaDerecha.y, startPosition.y + limiteAbajoArriba.y);
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

        // 1. Leer entrada del jugador (ejes X e Y)
        Vector2 input = moveAction.ReadValue<Vector2>();

        // 2. Calcular la velocidad objetivo para ambos ejes
        Vector2 targetVelocity = input * moveSpeed;

        // 3. Aplicar aceleración hacia la velocidad objetivo
        Vector2 currentVelocity = playerRigidbody.linearVelocity;
        Vector2 newVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        playerRigidbody.linearVelocity = newVelocity;

        // 4. Limitar la posición del Rigidbody dentro de las fronteras calculadas
        Vector2 currentPosition = playerRigidbody.position;
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y)
        );

        // Si sobrepasa el límite, forzamos la posición y detenemos el impulso en esa dirección
        playerRigidbody.position = clampedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        Debug.Log("Game Over!!");

        isGameOver = true;
        moveAction.Disable();
        playerRigidbody.linearVelocity = Vector2.zero;
        Time.timeScale = 0f;
    }

    // Dibujar los límites en la ventana de Scene de Unity para verlos en tiempo real
    private void OnDrawGizmosSelected()
    {
        Vector2 origin = Application.isPlaying ? (minBounds + maxBounds) / 2f : (Vector2)transform.position;
        Vector2 size = Application.isPlaying ? (maxBounds - minBounds) : new Vector2(limiteIzquierdaDerecha.x + limiteIzquierdaDerecha.y, limiteAbajoArriba.x + limiteAbajoArriba.y);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(origin, size);
    }
}