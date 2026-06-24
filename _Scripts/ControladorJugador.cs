using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorJugador : MonoBehaviour
{
    public float velocidadBase = 25f;
    public float multiplicadorVelocidad = 1.5f;
    public SpriteRenderer spritePersonaje;
    public ControladorPistola scriptPistola;

    private Rigidbody2D rb;
    private Animator anim;
    private VidaJugador vida;
    private MejorasJugador mejoras;
    private Vector2 mov;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        vida = GetComponent<VidaJugador>();
        mejoras = GetComponent<MejorasJugador>();

        rb.freezeRotation = true;
    }

    void OnMove(InputValue value) { if (!vida.estaMuerto) mov = value.Get<Vector2>(); }
    void OnFire(InputValue value) { if (scriptPistola != null && !vida.estaMuerto) scriptPistola.estaDisparando = value.isPressed; }

    void Update()
    {
        if (vida != null && vida.estaMuerto)
        {
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.enabled = false;
            return;
        }

        if (anim != null)
        {
            anim.SetFloat("Horizontal", mov.x);
            anim.SetFloat("Vertical", mov.y);
            anim.SetFloat("Speed", mov.magnitude);
        }

        if (spritePersonaje != null)
        {
            if (mov.x > 0.1f) spritePersonaje.flipX = false;
            else if (mov.x < -0.1f) spritePersonaje.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (vida != null && vida.estaMuerto) return;

        float velocidadCalculada = velocidadBase;

        if (mejoras != null && mejoras.mejoraVelocidad)
        {
            velocidadCalculada = velocidadBase * multiplicadorVelocidad;
        }

        rb.linearVelocity = mov.normalized * velocidadCalculada;
    }
}