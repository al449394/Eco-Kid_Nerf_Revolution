using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variable pública para que puedas ajustar la velocidad desde Unity sin abrir el código
    public float velocidad = 5f; 

    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Start()
    {
        // Al darle al Play, el código busca el Rigidbody 2D del personaje y se "engancha" a él
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. LEER LOS CONTROLES (WASD o Flechas)
        // Usamos GetAxisRaw para que el movimiento sea instantáneo (como en TBOI), sin aceleración.
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        // 2. NORMALIZAR EL MOVIMIENTO
        // Esto es un truco vital: evita que el personaje corra más rápido cuando te mueves en diagonal.
        movimiento = movimiento.normalized;
    }

    void FixedUpdate()
    {
        // 3. APLICAR LA FÍSICA
        // FixedUpdate es el lugar correcto para mover cosas con Rigidbody.
        // Multiplicamos la dirección por la velocidad para mover al personaje.
        rb.linearVelocity = movimiento * velocidad;
    }
}