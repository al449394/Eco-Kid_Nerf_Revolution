using UnityEngine;

public class CamaraHorizontal : MonoBehaviour
{
    public Transform objetivo;
    public float suavizado = 0.125f;
    public float limiteIzquierdo;
    public float limiteDerecho;

    private Vector3 velocidad = Vector3.zero;

    //late update por que debe ser lo ultimo que se haga despues de que el jugador se mueva
    //si no se iria calculando constantemente y crearia un temblor
    void LateUpdate()
    {
        if (objetivo == null) return;

        float targetX = Mathf.Clamp(objetivo.position.x, limiteIzquierdo, limiteDerecho);
        Vector3 destino = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, destino, ref velocidad, suavizado);
    }
}