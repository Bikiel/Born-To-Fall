using UnityEngine;

public class RepeatBackgroundJam2 : MonoBehaviour
{
    public Vector3 startPos;
    public float repeatWidth;
    public float extravalue=0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        //repeatWidth = GetComponent<BoxCollider>().size.y / 2;
        repeatWidth = GetComponent<BoxCollider>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > startPos.y + repeatWidth + extravalue)
        {
            transform.position = startPos;
        }
    }
}
