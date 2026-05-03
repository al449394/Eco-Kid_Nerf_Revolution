using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadBase = 25f; // Ajustada a 25 ya que 12 era muy lento

    [Header("Referencias Visuales")]
    public SpriteRenderer spritePersonaje; // Arrastra aquí el objeto "Cuerpo_Visible"

    private Rigidbody2D rb;
    private Animator anim;
    private MejorasJugador mejoras;
    private Vector2 mov;

    void Start()
    {
        // Referencias básicas
        rb = GetComponent<Rigidbody2D>();
        mejoras = GetComponent<MejorasJugador>();

        // CAMBIO CLAVE: Buscamos el Animator en los hijos (Cuerpo_Visible)
        anim = GetComponentInChildren<Animator>();

        // Configuración física para evitar caídas o rotaciones raras
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // Verificación en consola
        if (anim == null)
        {
            Debug.LogError("No se encontró Animator en el Player ni en sus hijos. Revisa la jerarquía.");
        }
    }

    void Update()
    {
        // 1. CAPTURAR ENTRADA (Teclado)
        mov.x = Input.GetAxisRaw("Horizontal");
        mov.y = Input.GetAxisRaw("Vertical");

        // 2. ENVIAR DATOS AL ANIMATOR
        if (anim != null)
        {
            // Enviamos Horizontal y Vertical para el Blend Tree
            anim.SetFloat("Horizontal", mov.x);
            anim.SetFloat("Vertical", mov.y);

            // Enviamos Speed: 1 si se mueve, 0 si está quieto
            // Esto asegura que la transición se active siempre
            float valorSpeed = (mov.magnitude > 0.01f) ? 1f : 0f;
            anim.SetFloat("Speed", valorSpeed);
        }

        // 3. GIRAR EL SPRITE (FLIP)
        if (spritePersonaje != null)
        {
            if (mov.x > 0) spritePersonaje.flipX = false; // Mira a la derecha
            else if (mov.x < 0) spritePersonaje.flipX = true; // Mira a la izquierda
        }
    }

    void FixedUpdate()
    {
        // 4. MOVIMIENTO FÍSICO
        float vFinal = velocidadBase;

        // Aplicamos mejora de velocidad si existe
        if (mejoras != null && mejoras.mejoraVelocidad)
        {
            vFinal *= 1.6f;
        }

        // Aplicamos la velocidad al Rigidbody2D
        // .normalized evita que corra más rápido al ir en diagonal
        rb.linearVelocity = mov.normalized * vFinal;
    }
}