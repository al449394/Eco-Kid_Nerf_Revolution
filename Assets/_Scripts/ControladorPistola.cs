using UnityEngine;

public class ControladorPistola : MonoBehaviour
{
    [Header("Ajustes de Disparo")]
    public GameObject prefabBala;    // El objeto de la bala que se disparará
    public Transform puntoDisparo;  // El objeto vacío en la punta del cañón
    public float cadenciaBase = 2.0f; // Tiempo de espera entre disparos inicial

    [Header("Referencias")]
    public MejorasJugador mejoras;  // Referencia al script de mejoras del jugador

    private float tiempoSiguienteDisparo = 0f; // Reloj interno para controlar la cadencia
    private float anguloRaton = 0f;            // El ángulo hacia donde apunta el mouse

    void Start()
    {
        // Si no hemos arrastrado las mejoras en el Inspector, las busca automáticamente en el objeto padre
        if (mejoras == null) mejoras = GetComponentInParent<MejorasJugador>();
    }

    void Update()
    {
        // Si no hay mejoras, no hacemos nada para evitar errores en la consola
        if (mejoras == null) return;

        // 1. OBTENER POSICIÓN DEL RATÓN
        // Convertimos la posición del ratón de píxeles de pantalla a coordenadas del mundo 2D
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionRaton.z = 0f; // Forzamos Z a 0 porque es un juego 2D

        // 2. CALCULAR DIRECCIÓN
        // Restamos la posición del ratón menos la de la pistola para obtener el vector dirección
        Vector3 direccion = posicionRaton - transform.position;

        // 3. CALCULAR ÁNGULO
        // Atan2 nos da el ángulo en radianes, luego lo pasamos a grados (Rad2Deg)
        anguloRaton = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 4. ROTACIÓN VISUAL (Aquí está el truco)
        // Usamos la rotación del mundo para que no le afecte cómo gire el padre.
        // Si el dibujo de tu pistola mira a la izquierda, dejamos el + 180f.
        // Si ahora te apunta al revés, cambia 180f por 0f.
        transform.rotation = Quaternion.Euler(0, 0, anguloRaton + 0f);

        // 5. CONTROL DE CADENCIA
        // Si tiene la metralleta, el tiempo entre balas es casi nada (0.15s)
        float cadenciaFinal = cadenciaBase;
        if (mejoras.tieneMetralleta) cadenciaFinal = 0.15f;
        else if (mejoras.mejoraCadencia) cadenciaFinal = 0.6f; // Con mejora común dispara más rápido

        // 6. DISPARAR
        // Si pulsamos el botón de disparo y ha pasado el tiempo de espera...
        if (Input.GetButton("Fire1") && Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
            // Actualizamos el reloj para el próximo disparo
            tiempoSiguienteDisparo = Time.time + cadenciaFinal;
        }
    }

    void Disparar()
    {
        // Creamos la bala central. Usamos el anguloRaton puro (sin el +180) 
        // para que la bala salga hacia el cursor, no hacia donde mira el dibujo.
        InstanciarBala(anguloRaton);

        // Si tenemos disparo triple, creamos dos balas a los lados (15 grados de diferencia)
        if (mejoras.tieneDisparoTriple)
        {
            InstanciarBala(anguloRaton + 15f);
            InstanciarBala(anguloRaton - 15f);
        }
    }

    void InstanciarBala(float anguloFinal)
    {
        if (prefabBala != null && puntoDisparo != null)
        {
            // Creamos (Instanciamos) la bala en la punta del cañón con la rotación del ratón
            Instantiate(prefabBala, puntoDisparo.position, Quaternion.Euler(0, 0, anguloFinal));
        }
    }
}