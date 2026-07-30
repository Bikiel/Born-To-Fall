using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Control3D : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float direccionX = 0f;

        // 1. Entrada por Teclado (Nuevo Input System)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                direccionX = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                direccionX = 1f;
            }
        }

        // 2. Entrada por Pantalla Táctil
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (touchPos.x < Screen.width / 2f)
            {
                direccionX = -1f;
            }
            else
            {
                direccionX = 1f;
            }
        }
        // 3. Fallback a Mouse (por si estás probando con el clic en PC)
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (mousePos.x < Screen.width / 2f)
            {
                direccionX = -1f;
            }
            else
            {
                direccionX = 1f;
            }
        }

        // Aplicar velocidad al Rigidbody 3D en el eje X
        rb.linearVelocity = new Vector3(direccionX * velocidad, rb.linearVelocity.y, rb.linearVelocity.z);
    }
}