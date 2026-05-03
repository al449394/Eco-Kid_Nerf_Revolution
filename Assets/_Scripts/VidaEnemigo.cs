using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    public float vidaActual = 8f;
    public float vidaMaxima = 8f;

    [Header("Referencias")]
    public Animator anim; // Arrastra aquí el Animator del enemigo
    public GameObject chapaPrefab; // Aquí irá tu dibujo de la chapa (Prefab)

    private bool haMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        // Si no arrastras el animator, el código intenta buscarlo solo
        if (anim == null) anim = GetComponent<Animator>();
    }

    public void TomarDanio(float cantidad)
    {
        if (haMuerto) return; // Si ya está muerto, no hace nada más

        vidaActual -= cantidad;
        Debug.Log("Enemigo recibe daño. Vida restante: " + vidaActual);

        // LANZAR ANIMACIÓN DE GOLPE
        if (anim != null)
        {
            // "RecibirGolpe" debe ser el nombre exacto del Trigger en tu Animator
            anim.SetTrigger("RecibirGolpe");
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        haMuerto = true;

        // Crea una chapa justo donde estaba el enemigo
        if (chapaPrefab != null)
        {
            Instantiate(chapaPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.1f);
    }
}