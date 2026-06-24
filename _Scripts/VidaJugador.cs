using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;

public class VidaJugador : MonoBehaviour
{
    [Header("Estadisticas")]
    public float vidaActual = 100f;
    public float vidaMaxima = 100f;
    public bool estaMuerto = false;

    [Header("Interfaz (Canvas)")]
    public UnityEngine.UI.Image barraVida;

    [Header("Efectos")]
    public SpriteRenderer spriteRenderer;
    public float tiempoInmunidad = 1.0f;
    public float velocidadParpadeo = 0.1f; // Tiempo entre parpadeos
    private bool esInvulnerable = false;

    [Header("Muerte (UI Toolkit)")]
    public UIDocument deathScreenUI;

    void Start()
    {
        vidaActual = vidaMaxima;
        if (deathScreenUI != null) deathScreenUI.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void RecibirDaño(float cantidad)
    {
        if (estaMuerto || esInvulnerable) return;

        vidaActual -= cantidad;
        if (barraVida != null) barraVida.fillAmount = vidaActual / vidaMaxima;

        if (vidaActual <= 0) Morir();
        else StartCoroutine(EfectoParpadeo()); // Llamamos a la nueva corrutina
    }

    // CORRUTINA DEFINITIVA DE PARPADEO
    IEnumerator EfectoParpadeo()
    {
        esInvulnerable = true;
        float tiempoPasado = 0;

        // Guardamos el color original por si acaso tienes un tinte puesto
        Color colorOriginal = spriteRenderer.color;
        Color colorTransparente = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0.2f);

        while (tiempoPasado < tiempoInmunidad)
        {
            // Cambiamos el color a casi invisible
            if (spriteRenderer != null) spriteRenderer.color = colorTransparente;
            yield return new WaitForSeconds(velocidadParpadeo);

            // Cambiamos el color a normal
            if (spriteRenderer != null) spriteRenderer.color = colorOriginal;
            yield return new WaitForSeconds(velocidadParpadeo);

            tiempoPasado += (velocidadParpadeo * 2);
        }

        // Al terminar, nos aseguramos de que el color vuelve a ser el original
        if (spriteRenderer != null) spriteRenderer.color = colorOriginal;
        esInvulnerable = false;
    }

    void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;

        Time.timeScale = 0f; // Congela todo el juego

        if (deathScreenUI != null)
        {
            deathScreenUI.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}