using UnityEngine;

public class Botella : MonoBehaviour
{
    public float velocidadMover = 8f;
    public float velocidadGiro = 720f;
    public int danioFases = 1;

    private Vector2 direccion;
    private bool configurada = false;

    public void Configurar(Vector2 dirNueva)
    {
        direccion = dirNueva;
        configurada = true;
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        if (!configurada) return;

        transform.Translate(direccion * velocidadMover * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, velocidadGiro * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Aplicamos daño
            VidaJugador scriptVida = collision.GetComponent<VidaJugador>();
            if (scriptVida != null)
            {
                scriptVida.RecibirDaño(danioFases);
            }

            // Destrucción inmediata para evitar que atraviese al jugador
            Destroy(gameObject);
        }
        //si choca con la bala
        else if (collision.CompareTag("Bala"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
        //si choca con otra cosa
        else if (collision.gameObject.name.Contains("Muro") ||
                 collision.gameObject.name.Contains("Suelo") ||
                 collision.gameObject.name.Contains("Limites"))
        {
            Destroy(gameObject);
        }
    }
}