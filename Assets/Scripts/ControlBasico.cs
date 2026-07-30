using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControlBasico : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float direccionX = 0f;

        // 1. Entrada por Teclado (Flechas / Teclas A y D)
        direccionX = Input.GetAxisRaw("Horizontal");

        // 2. Entrada por Pantalla Táctil / Móvil
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Si toca el lado izquierdo de la pantalla va a la izq (-1), si toca el derecho va a la der (1)
            if (touch.position.x < Screen.width / 2f)
            {
                direccionX = -1f;
            }
            else
            {
                direccionX = 1f;
            }
        }

        // Aplicar movimiento directamente a la velocidad del Rigidbody2D
        rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);
    }
}
