using UnityEngine;
using TMPro;

public class ManejadorChapas : MonoBehaviour
{
    [Header("Economía")]
    public int chapasActuales = 0;
    public TextMeshProUGUI textoChapas;

    void Start()
    {
        ActualizarUI();
    }

    public void AñadirChapa(int cantidad)
    {
        chapasActuales += cantidad;
        Debug.Log("Chapas totales: " + chapasActuales);
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (textoChapas != null)
        {
            textoChapas.text = chapasActuales.ToString();
        }
    }
}