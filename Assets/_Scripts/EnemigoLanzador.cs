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

    void Start()
    {
        posicionInicial = transform.position;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) jugador = playerObj.transform;
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (jugador == null) return;

        // COMPROBACIÓN CRÍTICA: ¿Está reproduciendo la animación de ataque?
        // Reemplaza "Enemigo1_ataque" por el nombre EXACTO de tu clip en el Animator
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Enemigo1_ataque"))
        {
            MirarAlJugador(); // Solo mira al jugador, no se mueve
            return; // Detiene el resto del script (no patrulla)
        }

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
        {
            GestionarAtaque();
        }
        else
        {
            Patrullar();
        }
    }

    void Patrullar()
    {
        anim.SetBool("isWalking", true);
        transform.Translate(Vector2.right * direccionPatrulla * velocidadPatrulla * Time.deltaTime);

        // Voltear hacia donde camina
        transform.localScale = new Vector3(direccionPatrulla * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Vector3.Distance(posicionInicial, transform.position) >= distanciaPatrulla)
        {
            direccionPatrulla *= -1;
            posicionInicial = transform.position;
        }
    }

    void GestionarAtaque()
    {
        // Forzamos que la animación de caminar se apague YA
        anim.SetBool("isWalking", false);

        MirarAlJugador();

        if (Time.time >= tiempoSiguienteAtaque)
        {
            anim.SetTrigger("atacar");
            tiempoSiguienteAtaque = Time.time + cadenciaAtaque;
        }
    }

    void MirarAlJugador()
    {
        if (jugador.position.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // FUNCIÓN PARA EL ANIMATION EVENT
    public void LanzarBotella()
    {
        if (prefabBotella != null && puntoLanzamiento != null)
        {
            GameObject botella = Instantiate(prefabBotella, puntoLanzamiento.position, Quaternion.identity);
            Vector2 dir = (jugador.position - puntoLanzamiento.position).normalized;
            botella.GetComponent<Botella>().Configurar(dir);
            Debug.Log("¡Botella lanzada desde el evento!");
        }
        else
        {
            Debug.LogWarning("Falta el Prefab de la botella o el Punto de Lanzamiento en el Inspector.");
        }
    }
}