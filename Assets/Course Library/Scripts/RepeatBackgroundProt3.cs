using UnityEngine;

public class RepeatBackgroundProt3 : MonoBehaviour
{
    public Vector3 startPos;
    public float repeatWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.position.x < startPos.x - repeatWidth)
        if (transform.position.y > startPos.y - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
