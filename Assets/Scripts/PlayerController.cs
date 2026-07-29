using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 25f;

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

        float horizontalInput = moveAction.ReadValue<Vector2>().x;
        float targetVelocityX = horizontalInput * moveSpeed;
        float velocityX = Mathf.MoveTowards(
            playerRigidbody.linearVelocity.x,
            targetVelocityX,
            acceleration * Time.fixedDeltaTime);

        playerRigidbody.linearVelocity = new Vector2(velocityX, playerRigidbody.linearVelocity.y);
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
}
