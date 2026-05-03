using UnityEngine;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    // 4 es 100%, 0 es muerto
    public int faseActual = 4;
    public Image imagenRelleno;

    // Aquí arrastras tus 4 dibujos
    public Sprite fase_100_completa; // La de 3 colores
    public Sprite fase_70_amarillo_rojo;
    public Sprite fase_40_todo_rojo;
    public Sprite fase_10_poco_rojo;

    void Start()
    {
        ActualizarUI();
    }

    public void RecibirDanio(int cantidad)
    {
        faseActual -= cantidad;
        faseActual = Mathf.Clamp(faseActual, 0, 4);

        ActualizarUI();

        if (faseActual <= 0) Morir();
    }

    void ActualizarUI()
    {
        // Si no hay vida, desactivamos la imagen para que se vea el fondo verde oscuro
        if (faseActual <= 0)
        {
            imagenRelleno.enabled = false;
            return;
        }

        imagenRelleno.enabled = true;

        // Cambiamos el dibujo según la fase
        if (faseActual == 4) imagenRelleno.sprite = fase_100_completa;
        else if (faseActual == 3) imagenRelleno.sprite = fase_70_amarillo_rojo;
        else if (faseActual == 2) imagenRelleno.sprite = fase_40_todo_rojo;
        else if (faseActual == 1) imagenRelleno.sprite = fase_10_poco_rojo;
    }

    void Morir()
    {
        Debug.Log("Has muerto. Solo queda el fondo verde oscuro.");
        // Aquí lógica de reinicio
    }
}