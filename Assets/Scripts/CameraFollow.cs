using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float offsetY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsetY = transform.position.y - target.position.y;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            target.position.y + offsetY,
            transform.position.z
        );
    }
}
