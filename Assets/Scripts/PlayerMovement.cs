using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f; 

    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        //para evitar que el personaje corra mas rapido al ir en diagonal
        movimiento = movimiento.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movimiento * velocidad;
    }
}
