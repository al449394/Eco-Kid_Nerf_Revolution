using UnityEngine;

public class Chapa : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo la recogemos si el que toca es el jugador
        if (collision.CompareTag("Player"))
        {
            ManejadorChapas manager = collision.GetComponent<ManejadorChapas>();

            if (manager != null)
            {
                manager.AñadirChapa(1);
                Destroy(gameObject); // La chapa desaparece al cogerla
            }
        }
    }
}