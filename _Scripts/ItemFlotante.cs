using UnityEngine;

public class ItemFlotante : MonoBehaviour
{
    public float velocidadSubida = 3f;
    public float tiempoVida = 1.5f;
    private SpriteRenderer sr;
    private float crono = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(Vector3.up * velocidadSubida * Time.deltaTime);
        if (sr != null)
        {
            crono += Time.deltaTime;
            //este es el efecto de transparencia
            float alfa = Mathf.Lerp(1f, 0f, crono / tiempoVida);
            //sin modificar ni el color rojo, ni el verde, ni el azul, solo el canal alfa
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alfa);
        }
    }
}