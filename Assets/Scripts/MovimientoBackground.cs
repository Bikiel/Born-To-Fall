using UnityEngine;

public class MovimientoBackground : MonoBehaviour
{

   
        [Header("Límites de Movimiento del Fondo")]
        [Tooltip("Punto máximo en Y al que sube el fondo")]
        public float limiteSubidaY = 0f;

        [Tooltip("Punto mínimo en Y al que baja el fondo")]
        public float limiteBajadaY = -500f;

        [Header("Velocidades de Movimiento")]
        [Tooltip("Velocidad máxima de movimiento")]
        public float velocidadMaxima = 20f;

        [Tooltip("Distancia antes de llegar al límite donde empieza a desacelerar suavemente")]
        public float distanciaFrenado = 20f;

        [Tooltip("Velocidad mínima para garantizar que toque el límite sin congelarse")]
        public float velocidadMinima = 1.5f;

        [Header("Lectura en Tiempo Real")]
        [Tooltip("Muestra la velocidad actual en el Inspector")]
        public float velocidadActual = 0f;

        // Control interno de estado
        private bool subiendo = true;

        void Update()
        {
            EvaluarEstadoYVelocidad();
            MoverFondo();
        }

        private void EvaluarEstadoYVelocidad()
        {
            float objetivoVelocidad = velocidadMaxima;

            if (subiendo)
            {
                // Calculamos la distancia restante hacia el límite superior
                float distanciaAlLimite = limiteSubidaY - transform.position.y;

                // Si entra en la zona de frenado, calculamos la desaceleración curva (SmoothStep)
                if (distanciaAlLimite <= distanciaFrenado)
                {
                    float factor = Mathf.Clamp01(distanciaAlLimite / distanciaFrenado);
                    objetivoVelocidad = Mathf.Lerp(velocidadMinima, velocidadMaxima, Mathf.SmoothStep(0f, 1f, factor));
                }

                // Al estar muy cerca del límite o pasarlo: ajustamos posición y cambiamos a bajar
                if (distanciaAlLimite <= 0.1f || transform.position.y >= limiteSubidaY)
                {
                    transform.position = new Vector3(transform.position.x, limiteSubidaY, transform.position.z);
                    subiendo = false;
                }
            }
            else // Bajando
            {
                // Calculamos la distancia restante hacia el límite inferior
                float distanciaAlLimite = transform.position.y - limiteBajadaY;

                // Si entra en la zona de frenado, desaceleramos suavemente
                if (distanciaAlLimite <= distanciaFrenado)
                {
                    float factor = Mathf.Clamp01(distanciaAlLimite / distanciaFrenado);
                    objetivoVelocidad = Mathf.Lerp(velocidadMinima, velocidadMaxima, Mathf.SmoothStep(0f, 1f, factor));
                }

                // Al estar muy cerca del límite o pasarlo: ajustamos posición y cambiamos a subir
                if (distanciaAlLimite <= 0.1f || transform.position.y <= limiteBajadaY)
                {
                    transform.position = new Vector3(transform.position.x, limiteBajadaY, transform.position.z);
                    subiendo = true;
                }
            }

            // Transición fluida de la velocidad actual hacia la velocidad objetivo
            velocidadActual = Mathf.MoveTowards(velocidadActual, objetivoVelocidad, Time.deltaTime * (velocidadMaxima * 2f));
        }

        private void MoverFondo()
        {
            // Se desplaza en el espacio global de forma vertical constante
            Vector3 direccion = subiendo ? Vector3.up : Vector3.down;
            transform.Translate(direccion * velocidadActual * Time.deltaTime, Space.World);
        }
    }