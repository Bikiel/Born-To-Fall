using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float leftBound = -15;
    public float speed = 30;
    public PlayerControllerProy3 playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerProy3>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.left * Time.deltaTime * speed);
        if (playerControllerScript.gameOver == false)
        {
            // transform.Translate(Vector3.left * Time.deltaTime * speed);
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
        }

        if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
