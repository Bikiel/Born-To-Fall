using UnityEngine;

public class Gravedad : MonoBehaviour
{
    [Header("Límites de Caída y Subida")]
    [Tooltip("Distancia en Y hacia abajo donde invierte gravedad y rota 180°")]
    public float limiteCaida = -10f;

    [Tooltip("Punto superior en Y donde vuelve a caer y regresa rotación a 0°")]
    public float limiteSubida = 0f;

    [Header("Fuerza de Gravedad")]
    public float fuerzaGravedad = 9.81f;

    [Header("Rotación")]
    [Tooltip("Si está activo, la rotación será suave; si no, será instantánea.")]
    public bool rotacionSuave = true;
    public float velocidadRotacion = 10f;

    private Rigidbody rb;
    private bool cayendo = true;
    private Quaternion rotacionObjetivo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
        }
        else
        {
            Debug.LogError("No se encontró el componente Rigidbody en el objeto.");
        }

        // La rotación inicial por defecto es 0 en todos los ejes
        rotacionObjetivo = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        EvaluarLimites();
        AplicarRotacion();
    }

    void FixedUpdate()
    {
        AplicarGravedad();
    }

    private void EvaluarLimites()
    {
        // Al llegar abajo: Invertir gravedad y fijar objetivo a 180° en Z
        if (cayendo && transform.position.y <= limiteCaida)
        {
            cayendo = false;
            rotacionObjetivo = Quaternion.Euler(0, 0, 180f);
        }
        // Al llegar arriba: Restablecer gravedad y fijar objetivo a 0° en Z
        else if (!cayendo && transform.position.y >= limiteSubida)
        {
            cayendo = true;
            rotacionObjetivo = Quaternion.Euler(0, 0, 0f);
        }
    }

    private void AplicarRotacion()
    {
        if (rotacionSuave)
        {
            // Transición fluida de la rotación
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadRotacion);
        }
        else
        {
            // Giro instantáneo
            transform.rotation = rotacionObjetivo;
        }
    }

    private void AplicarGravedad()
    {
        if (rb == null) return;

        if (cayendo)
        {
            rb.AddForce(Vector3.down * fuerzaGravedad, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.up * fuerzaGravedad, ForceMode.Acceleration);
        }
    }
}