using UnityEngine;

public class Gravedad : MonoBehaviour
{
    [Header("Límites de Caída y Subida")]
    [Tooltip("Distancia en Y hacia abajo (ej: -10 para que invierta gravedad al llegar a Y = -10)")]
    public float limiteCaida = -10f;

    [Tooltip("Punto de inicio o tope en Y donde vuelve a caer")]
    public float limiteSubida = 0f;

    [Header("Fuerza de Gravedad")]
    [Tooltip("Magnitud de la gravedad (por defecto suele ser 9.81 o mayor si quieres caídas rápidas)")]
    public float fuerzaGravedad = 9.81f;

    private Rigidbody rb;
    private bool cayendo = true;

    void Start()
    {
        // Obtenemos el Rigidbody del personaje
        rb = GetComponent<Rigidbody>();

        // Desactivamos la gravedad del motor de física por defecto de Unity
        // para tener un control manual total sobre la aceleración vertical
        if (rb != null)
        {
            rb.useGravity = false;
        }
        else
        {
            Debug.LogError("No se encontró el componente Rigidbody en el objeto. ¡Asegúrate de tener uno agregado!");
        }
    }

    void Update()
    {
        EvaluarLimites();
    }

    void FixedUpdate()
    {
        AplicarGravedad();
    }

    private void EvaluarLimites()
    {
        // Si va cayendo y pasa del límite inferior (ej. Y <= -10) -> Invertir a subida
        if (cayendo && transform.position.y <= limiteCaida)
        {
            cayendo = false;
        }
        // Si va subiendo y llega/supera el límite superior (ej. Y >= 0) -> Volver a caer
        else if (!cayendo && transform.position.y >= limiteSubida)
        {
            cayendo = true;
        }
    }

    private void AplicarGravedad()
    {
        if (rb == null) return;

        if (cayendo)
        {
            // Gravedad normal hacia abajo (-Y)
            rb.AddForce(Vector3.down * fuerzaGravedad, ForceMode.Acceleration);
        }
        else
        {
            // Gravedad invertida hacia arriba (+Y)
            rb.AddForce(Vector3.up * fuerzaGravedad, ForceMode.Acceleration);
        }
    }
}