using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float initialRotationSpeed = 1.0f;

    void Start()
    {
        transform.position = new Vector3(3, 4, 1);
        transform.localScale = Vector3.one * 1.3f;
        
        Material material = Renderer.material;
        
        material.color = new Color(0.5f, 1.0f, 0.3f, 0.4f);
    }
    
    void Update()
    {

        transform.Rotate(10.0f * Time.deltaTime, 0.0f, 0.0f);
        /*Modifica el script cube.cs para que se realicen al menos dos cambios en el comportamiento del cubo. Algunas cosas que puedes considerar hacer son:
Cambiar la ubicación del cubo (Transform).
Cambiar el tamaño del cubo.
Cambiar el ángulo de rotación del cubo.
Cambiar la velocidad de rotación del cubo.
Cambiar el color del material del cubo.
Cambiar la opacidad del material del cubo.

Si te sientes preparado, intenta también lo siguiente:
Modifica cualquiera de los cambios anteriores para que cambien aleatoriamente cada vez que se reproduzca la Escena.
Agrega una funcionalidad extra al cubo. Por ejemplo, ¿cómo cambiarías el color del cubo con el tiempo?*/

        //Cambiar la velocidad de rotación del cubo.
        initialRotationSpeed = Mathf.PingPong(Time.time, 5.0f) + 1.0f; // Cambia la velocidad entre 1 y 6
        transform.rotation = Quaternion.Euler(10.0f * Time.time * initialRotationSpeed, 0.0f, 0.0f);


        //Cambiar la opacidad del material del cubo.
        Material material = Renderer.material;
         Color color = material.color;
         color.a = Mathf.PingPong(Time.time, 1.0f);
         material.color = color;
    }
}
