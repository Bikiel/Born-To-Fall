using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float horizontalLimit = 8.4f;

    private Rigidbody2D playerRigidbody;
    private bool isGameOver;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
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
        }
    }

    public void DisableMovement()
    {
        Debug.Log("Game Over!!");
        moveAction.Disable();
        playerRigidbody.linearVelocity = Vector2.zero;
    }
}
