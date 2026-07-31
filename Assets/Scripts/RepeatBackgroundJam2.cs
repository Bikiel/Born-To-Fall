using UnityEngine;

public class RepeatBackgroundJam2 : MonoBehaviour
{
    public Vector3 startPos;
    public float repeatWidth;
    public float extravalue = 0;
    public bool isCollider3D = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        //repeatWidth = GetComponent<BoxCollider>().size.y / 2;
        if (isCollider3D)
        {
            repeatWidth = GetComponent<BoxCollider>().size.y;
        }
        else
        {
            repeatWidth = GetComponent<BoxCollider2D>().size.y;
        }

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
