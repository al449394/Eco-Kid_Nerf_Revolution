using UnityEngine;

public class EnemigoLanzador : MonoBehaviour
{
    [Header("Configuracion")]
    public Animator anim;
    public Transform puntoLanzamiento;
    public GameObject prefabBotella;

    [Header("Ajustes")]
    public float velocidadPatrulla = 2f;
    public float distanciaPatrulla = 10f;
    public float rangoDeteccion = 7f;
    public float cadenciaAtaque = 2f;

    private Vector3 posicionInicial;
    private int direccionPatrulla = 1;
    private float tiempoSiguienteAtaque;
    private Transform jugador;
    private Rigidbody2D rb;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) jugador = playerObj.transform;

        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (jugador == null) return;

        // si esta con la animacion de daño se para
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Enemigo1_golpe"))
        {
            FrenarFisica();
            return;
        }

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // ataca si el jugador esta cerca
        if (distancia <= rangoDeteccion)
        {
            EjecutarModoAtaque();
        }
        // va caminando
        else
        {
            EjecutarModoPatrulla();
        }
    }

    void EjecutarModoPatrulla()
    {
        anim.SetBool("isWalking", true);

        // Movimiento
        transform.Translate(Vector2.right * direccionPatrulla * velocidadPatrulla * Time.deltaTime);

        // Giro de Sprite
        float escalaX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(direccionPatrulla * escalaX, transform.localScale.y, transform.localScale.z);

        // Cambio de sentido
        if (Vector3.Distance(posicionInicial, transform.position) >= distanciaPatrulla)
        {
            direccionPatrulla *= -1;
            posicionInicial = transform.position;
        }
    }

    void EjecutarModoAtaque()
    {
        // decimos al animator que pare de caminar
        anim.SetBool("isWalking", false);
        FrenarFisica();

        // mirar al jugador siempre
        float lado = (jugador.position.x > transform.position.x) ? 1 : -1;
        float escalaX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(lado * escalaX, transform.localScale.y, transform.localScale.z);

        // 4. Atacar por tiempo (Solo si no está ya en la animación de ataque)
        if (Time.time >= tiempoSiguienteAtaque && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemigo1_ataque"))
        {
            anim.SetTrigger("atacar");
            tiempoSiguienteAtaque = Time.time + cadenciaAtaque;
        }
    }

    void FrenarFisica()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    // Evento de animación
    public void LanzarBotella()
    {
        if (prefabBotella != null && puntoLanzamiento != null)
        {
            GameObject botella = Instantiate(prefabBotella, puntoLanzamiento.position, Quaternion.identity);
            Vector2 dir = (jugador.position - puntoLanzamiento.position).normalized;

            if (botella.GetComponent<Botella>() != null)
                botella.GetComponent<Botella>().Configurar(dir);
        }
    }
}