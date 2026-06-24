using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorPistola : MonoBehaviour
{
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float cadenciaBase = 0.5f;
    public MejorasJugador mejoras;

    private VidaJugador vidaJugador;
    private float tiempoSiguienteDisparo = 0f;
    [HideInInspector] public bool estaDisparando = false;

    void Start()
    {
        vidaJugador = GetComponentInParent<VidaJugador>();
        mejoras = GetComponentInParent<MejorasJugador>();
    }

    void Update()
    {
        // 1. EL CANDADO: Si el juego está en pausa/muerte, salimos inmediatamente.
        if (Time.timeScale == 0 || (vidaJugador != null && vidaJugador.estaMuerto))
        {
            estaDisparando = false;
            return;
        }

        // Si sueltas el click, dejamos de disparar
        if (!Mouse.current.leftButton.isPressed) estaDisparando = false;

        // --- LÓGICA DE ROTACIÓN (Sigue al ratón) ---
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        posicionRaton.z = 0f;
        Vector3 direccion = posicionRaton - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // --- LÓGICA DE CADENCIA Y DISPARO ---
        if (estaDisparando && Time.time >= tiempoSiguienteDisparo)
        {
            // Calculamos la cadencia (velocidad de disparo)
            float cadenciaFinal = cadenciaBase;
            if (mejoras != null)
            {
                if (mejoras.tieneMetralleta) cadenciaFinal = 0.1f;
                else if (mejoras.mejoraCadencia) cadenciaFinal = cadenciaBase / 2;
            }

            // EJECUCIÓN DEL DISPARO (Aquí corregimos el fallo de las gafas)
            // IMPORTANTE: Revisa si en tu script "MejorasJugador" el bool se llama "tieneDisparoTriple" o "mejoraDisparoTriple"
            if (mejoras != null && mejoras.tieneDisparoTriple)
            {
                // Disparo Triple (Gafas): Una al centro, otra a la izquierda (+15º) y otra a la derecha (-15º)
                InstanciarBala(angulo);
                InstanciarBala(angulo + 15f);
                InstanciarBala(angulo - 15f);
            }
            else
            {
                // Disparo Normal
                InstanciarBala(angulo);
            }

            // Aplicamos el tiempo de espera para el siguiente tiro
            tiempoSiguienteDisparo = Time.time + cadenciaFinal;
        }
    }

    void InstanciarBala(float angulo)
    {
        if (prefabBala != null && puntoDisparo != null)
            Instantiate(prefabBala, puntoDisparo.position, Quaternion.Euler(0, 0, angulo));
    }
}