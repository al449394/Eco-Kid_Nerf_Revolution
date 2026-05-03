using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Ajustes Base")]
    public float velocidadBase = 20f;
    public float danioBase = 3f;

    [Header("Ajustes de Rastreo (Homing)")]
    public float fuerzaGiro = 5f;
    public float radioDeteccion = 10f;

    [Header("Estado y Mejoras")]
    public bool esBalaHija = false;
    private MejorasJugador mejoras;
    private Rigidbody2D rb;
    private Transform objetivo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) mejoras = player.GetComponent<MejorasJugador>();

        // 1. TAMAÑO ACUMULATIVO
        float escalaFinal = 0.3f;
        if (mejoras != null)
        {
            if (mejoras.tieneMetralleta) escalaFinal -= 0.15f;
            if (mejoras.mejoraDanio) escalaFinal += 0.25f;

            float tiempoVida = mejoras.mejoraAlcance ? 4f : 1.5f;
            Destroy(gameObject, tiempoVida);
        }
        transform.localScale = new Vector3(escalaFinal, escalaFinal, 1f);

        rb.linearVelocity = transform.right * velocidadBase;

        if (mejoras != null && mejoras.tieneTeledirigido)
        {
            BuscarObjetivoCercano();
        }
    }

    void FixedUpdate()
    {
        if (mejoras != null && mejoras.tieneTeledirigido)
        {
            if (objetivo == null) BuscarObjetivoCercano();
            else
            {
                Vector2 direccionDeseada = (Vector2)objetivo.position - rb.position;
                direccionDeseada.Normalize();
                float cantidadGiro = Vector3.Cross(direccionDeseada, transform.right).z;
                rb.angularVelocity = -cantidadGiro * fuerzaGiro * 200f;
                rb.linearVelocity = transform.right * velocidadBase;
            }
        }
    }

    void BuscarObjetivoCercano()
    {
        float distanciaMasCercana = radioDeteccion;
        objetivo = null;
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");

        foreach (GameObject enemigo in enemigos)
        {
            float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                objetivo = enemigo.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ELIMINADO BalaEnemigo para evitar el error de Tag
        if (collision.CompareTag("Bala") || collision.CompareTag("Player"))
        {
            return;
        }

        if (collision.CompareTag("Enemigo"))
        {
            float danioFinal = danioBase;
            if (mejoras != null)
            {
                if (mejoras.tieneMetralleta) danioFinal = 1.5f;
                if (mejoras.mejoraDanio) danioFinal += 4f;
            }
            collision.GetComponent<VidaEnemigo>()?.TomarDanio(danioFinal);
            DividirEnT(collision);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Suelo"))
        {
            if (mejoras != null && mejoras.tieneRebote)
            {
                rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, Vector2.up);
                float angulo = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angulo);
                objetivo = null;
            }
            else
            {
                DividirEnT(collision);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Botella"))
        {
            DividirEnT(collision);
            Destroy(gameObject);
        }
    }

    void DividirEnT(Collider2D colisionOriginal)
    {
        if (mejoras != null && mejoras.tieneDisparoEnT && !esBalaHija)
        {
            for (int i = 0; i < 2; i++)
            {
                float modAngulo = (i == 0) ? 90f : -90f;
                Quaternion rotHija = transform.rotation * Quaternion.Euler(0, 0, modAngulo);
                GameObject objetoHijo = Instantiate(gameObject, transform.position, rotHija);

                Bala scriptHijo = objetoHijo.GetComponent<Bala>();
                if (scriptHijo != null)
                {
                    scriptHijo.esBalaHija = true;
                    Rigidbody2D rbHijo = objetoHijo.GetComponent<Rigidbody2D>();
                    rbHijo.linearVelocity = objetoHijo.transform.right * velocidadBase;

                    if (colisionOriginal != null)
                        Physics2D.IgnoreCollision(objetoHijo.GetComponent<Collider2D>(), colisionOriginal);

                    objetoHijo.transform.position += objetoHijo.transform.right * 0.4f;
                }
            }
        }
    }
}