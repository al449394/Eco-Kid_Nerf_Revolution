using UnityEngine;

public class InteraccionBasura : MonoBehaviour
{
    [Header("Configuración Visual")]
    public GameObject plantillaIcono;
    public Sprite[] comunes;
    public Sprite[] epicos;
    public Sprite[] legendarios;

    [Header("Ajustes de Tienda")]
    public int costeMejora = 6;

    private MejorasJugador mejoras;
    private ManejadorChapas fichasControlador;
    private bool jugadorCerca = false;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            mejoras = player.GetComponent<MejorasJugador>();
            fichasControlador = player.GetComponent<ManejadorChapas>();
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            IntentarCompra();
        }
    }

    void IntentarCompra()
    {
        if (mejoras == null || fichasControlador == null) return;

        // Primero miramos si el jugador ya lo tiene TODO al máximo
        if (TieneTodoAlMaximo())
        {
            Debug.Log("¡Ya eres un dios! No quedan más mejoras en la basura.");
            return;
        }

        if (fichasControlador.chapasActuales >= costeMejora)
        {
            CanjearAleatorio(); // El cobro lo hacemos dentro si encontramos algo nuevo
        }
    }

    void CanjearAleatorio()
    {
        Sprite spriteElegido = null;
        string nombreProcesado = "";
        bool encontradaNueva = false;
        int intentos = 0;

        // Intentamos buscar una mejora que NO tengas (máximo 30 intentos para evitar bloqueos)
        while (!encontradaNueva && intentos < 30)
        {
            float suerte = Random.Range(0f, 100f);

            if (suerte <= 60f) spriteElegido = comunes[Random.Range(0, comunes.Length)];
            else if (suerte <= 90f) spriteElegido = epicos[Random.Range(0, epicos.Length)];
            else spriteElegido = legendarios[Random.Range(0, legendarios.Length)];

            nombreProcesado = spriteElegido.name.ToLower().Trim();

            // Comprobamos si ya tienes esta mejora específica
            if (!YaTieneEstaMejora(nombreProcesado))
            {
                encontradaNueva = true;
            }
            intentos++;
        }

        if (encontradaNueva)
        {
            // Solo cobramos y activamos si es una mejora que no tenías
            fichasControlador.chapasActuales -= costeMejora;
            fichasControlador.ActualizarUI();

            ActivarMejoraDefinitiva(nombreProcesado);
            LanzarIcono(spriteElegido);
        }
        else
        {
            // Si después de 30 intentos no sale nada nuevo, es que las que quedan son muy raras
            // o tienes casi todo. ¡Mala suerte en esta tirada!
            Debug.Log("Esta vez solo había chatarra... (Salió algo repetido)");
        }
    }

    // Función que mira si el booleano ya está en true
    bool YaTieneEstaMejora(string nombre)
    {
        if (nombre.Contains("dmg")) return mejoras.mejoraDanio;
        if (nombre.Contains("firerate")) return mejoras.mejoraCadencia;
        if (nombre.Contains("range")) return mejoras.mejoraAlcance;
        if (nombre.Contains("speed")) return mejoras.mejoraVelocidad;
        if (nombre.Contains("bounce")) return mejoras.tieneRebote;
        if (nombre.Contains("t shoot")) return mejoras.tieneDisparoEnT;
        if (nombre.Contains("triple")) return mejoras.tieneDisparoTriple;
        if (nombre.Contains("homing")) return mejoras.tieneTeledirigido;
        if (nombre.Contains("machinegun")) return mejoras.tieneMetralleta;
        return false;
    }

    void ActivarMejoraDefinitiva(string nombre)
    {
        if (nombre.Contains("dmg")) mejoras.mejoraDanio = true;
        else if (nombre.Contains("firerate")) mejoras.mejoraCadencia = true;
        else if (nombre.Contains("range")) mejoras.mejoraAlcance = true;
        else if (nombre.Contains("speed")) mejoras.mejoraVelocidad = true;
        else if (nombre.Contains("bounce")) mejoras.tieneRebote = true;
        else if (nombre.Contains("t shoot")) mejoras.tieneDisparoEnT = true;
        else if (nombre.Contains("triple")) mejoras.tieneDisparoTriple = true;
        else if (nombre.Contains("homing")) mejoras.tieneTeledirigido = true;
        else if (nombre.Contains("machinegun")) mejoras.tieneMetralleta = true;
    }

    // Seguridad para no entrar en un bucle infinito si ya compró las 9
    bool TieneTodoAlMaximo()
    {
        return mejoras.mejoraDanio && mejoras.mejoraCadencia && mejoras.mejoraAlcance &&
               mejoras.mejoraVelocidad && mejoras.tieneRebote && mejoras.tieneDisparoEnT &&
               mejoras.tieneDisparoTriple && mejoras.tieneTeledirigido && mejoras.tieneMetralleta;
    }

    void LanzarIcono(Sprite s)
    {
        if (plantillaIcono != null)
        {
            GameObject icono = Instantiate(plantillaIcono, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            icono.GetComponent<SpriteRenderer>().sprite = s;
            icono.GetComponent<SpriteRenderer>().sortingOrder = 50;
            if (!icono.GetComponent<ItemFlotante>()) icono.AddComponent<ItemFlotante>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) jugadorCerca = true; }
    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorCerca = false; }
}