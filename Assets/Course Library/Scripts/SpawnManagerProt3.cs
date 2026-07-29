using UnityEngine;

public class SpawnManagerProt3 : MonoBehaviour
{
    private PlayerControllerProy3 playerControllerScript;
    public GameObject obstaclePrefab;
    public Vector3 spawnPos = new Vector3(25,0,0);
    public float startDelay = 2;
    public float repeatRate = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        //Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerProy3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle ()
    {

        if (playerControllerScript.gameOver == false) 
        {
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }

        
    }
}
