using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    public float vidaActual = 8f;
    public float vidaMaxima = 8f;

    [Header("Referencias")]
    public Animator anim;
    public GameObject chapaPrefab;

    private bool haMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        if (anim == null) anim = GetComponent<Animator>();
    }

    public void TomarDaño(float cantidad)
    {
        if (haMuerto) return;

        vidaActual -= cantidad;
        Debug.Log("Enemigo recibe daño. Vida restante: " + vidaActual);

        //animacion
        if (anim != null)
        {

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

        // dropea una chapa
        if (chapaPrefab != null)
        {
            Instantiate(chapaPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.1f);
    }
}