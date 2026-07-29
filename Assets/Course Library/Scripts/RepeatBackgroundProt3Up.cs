using UnityEngine;

public class RepeatBackgroundProt3Up : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;

    void Start()
    {
        startPos = transform.position;

        // Usamos bounds.size.x en lugar de GetComponent<BoxCollider>().size.x
        // 'bounds' calcula el tamaño real ocupado en el eje X del mundo, 
        // teniendo en cuenta la rotación y escala del objeto.
        repeatWidth = GetComponent<BoxCollider>().bounds.size.x / 2;
    }

    void Update()
    {
        // Se sigue evaluando la posición en X (horizontal)
        if (transform.position.y < startPos.y - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
