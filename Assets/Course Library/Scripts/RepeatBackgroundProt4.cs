using UnityEngine;

public class RepeatBackgroundProt4 : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatHeight;

    void Start()
    {
        // Guardamos la posición inicial
        startPos = transform.position;

        // Medimos la mitad de la altura (eje Y) del BoxCollider
        // Usamos bounds.size.y para considerar la escala aplicada al objeto
        repeatHeight = GetComponent<BoxCollider>().bounds.size.y / 2;
    }

    void Update()
    {
        // Si el fondo cae/baja por debajo del límite de su altura en Y
        if (transform.position.y < startPos.y - repeatHeight)
        {
            // Lo devolvemos a la posición inicial en Y
            transform.position = startPos;
        }
    }
}