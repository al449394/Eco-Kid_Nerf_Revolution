using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    //serializefield es para que a pesar de ser private aparezca en el inspector para poder editarlo
    [SerializeField] private string nombreSiguienteEscena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si lo que ha entrado en el camión es el Jugador
        if (collision.CompareTag("Player"))
        {
            GanarNivel();
        }
    }

    void GanarNivel()
    {
        Debug.Log("¡Nivel Completado!");

        if (!string.IsNullOrEmpty(nombreSiguienteEscena))
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
        else
        {
            Debug.LogWarning("No has puesto el nombre de la siguiente escena en el Inspector.");
        }
    }
}