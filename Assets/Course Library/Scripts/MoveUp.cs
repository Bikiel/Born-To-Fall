using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float leftBound = -15f;
    public float speed = 30f;
    public PlayerControllerProy3 playerControllerScript;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerProy3>();
    }

    void Update()
    {
        if (playerControllerScript != null && playerControllerScript.gameOver == false)
        {
            // Agregamos Space.World al final para que SIEMPRE mueva hacia la izquierda global,
            // sin importar cómo esté rotado el objeto en la escena.
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}