using UnityEngine;
using TMPro;

public class ManejadorChapas : MonoBehaviour
{
    [Header("Economía")]
    public int chapasActuales = 0;
    public TextMeshProUGUI textoChapas; // ¡ARRASTRA TU TEXTO AQUÍ EN EL INSPECTOR!

    void Start()
    {
        ActualizarUI();
    }

    public void AñadirChapa(int cantidad)
    {
        chapasActuales += cantidad;
        Debug.Log("Chapas totales: " + chapasActuales); // Para ver si sumas bien
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (textoChapas != null)
        {
            textoChapas.text = chapasActuales.ToString();
        }
        else
        {
            // Si sale este error, es que no has arrastrado el texto al inspector
            Debug.LogWarning("¡Falta asignar el Texto de las Chapas en el Inspector!");
        }
    }
}