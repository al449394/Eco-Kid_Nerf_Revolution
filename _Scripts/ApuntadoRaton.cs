using UnityEngine;

public class ApuntadoRaton : MonoBehaviour
{
    void Update()
    {
        // 1. Obtener la posición del ratón en la pantalla y convertirla al mundo del juego
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. Calcular la dirección: (Destino - Origen)
        // El origen es la posición de este objeto
        //Al usar .normalized, hacemos que la fuerza del vector sea 1. No nos importa la distancia, solo hacia dónde apunta.
        Vector2 direccion = (posicionRaton - transform.position).normalized;

        // Mathf.Atan2: Es una función de arco tangente. Le das la vertical (y) y la horizontal (x) y ella te dice qué ángulo forma.
        // Esto nos da el ángulo en radianes, así que lo pasamos a grados (* Rad2Deg)
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 4. Aplicar la rotación al objeto en el eje Z
        transform.rotation = Quaternion.Euler(0, 0, angulo);


        if (angulo > 90 || angulo < -90)
        {
            // Si apunta a la izquierda, invertimos el eje Y de la pistola
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            // Si apunta a la derecha, escala normal
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}