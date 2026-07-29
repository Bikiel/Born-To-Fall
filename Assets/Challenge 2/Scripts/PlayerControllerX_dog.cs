using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX_dog : MonoBehaviour
{
    public GameObject dogPrefab;
    public float spamTimer;
    public float spamTime=0f;

    // Update is called once per frame
    void Update()
    {
        
        if (spamTime <= 0f && Input.GetKeyDown(KeyCode.Space))
        //spamTimer = Time.time;
        {
            spamTime = spamTimer;
            // On spacebar press, send dog
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            }*/
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
        }
        else
        {
            //spamTimer -= Time.deltaTime;
            if(spamTime > 0f)
            {
                spamTime = spamTime - 1f;
            }
            
        }
    }
}
