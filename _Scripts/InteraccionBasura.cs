using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class InteraccionBasura : MonoBehaviour
{
    [Header("Configuración Visual")]
    public GameObject plantillaIcono;
    public Sprite[] comunes;
    public Sprite[] epicos;
    public Sprite[] legendarios;

    [Header("Ajustes de Cofre")]
    public int costeMejora = 2;

    private MejorasJugador mejoras;
    private ManejadorChapas fichasControlador;
    private bool jugadorCerca = false;

    private Dictionary<string, Func<bool>> comprobadorMejoras;
    private Dictionary<string, Action> activadorMejoras;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            mejoras = player.GetComponent<MejorasJugador>();
            fichasControlador = player.GetComponent<ManejadorChapas>();
            InicializarDiccionarios();
        }
    }

    void InicializarDiccionarios()
    {
        comprobadorMejoras = new Dictionary<string, Func<bool>>
        {
            { "dmg", () => mejoras.mejoraDanio },
            { "firerate", () => mejoras.mejoraCadencia },
            { "range", () => mejoras.mejoraAlcance },
            { "speed", () => mejoras.mejoraVelocidad },
            { "bounce", () => mejoras.tieneRebote },
            { "t shoot", () => mejoras.tieneDisparoEnT },
            { "triple", () => mejoras.tieneDisparoTriple },
            { "homing", () => mejoras.tieneTeledirigido },
            { "machinegun", () => mejoras.tieneMetralleta }
        };

        activadorMejoras = new Dictionary<string, Action>
        {
            { "dmg", () => mejoras.mejoraDanio = true },
            { "firerate", () => mejoras.mejoraCadencia = true },
            { "range", () => mejoras.mejoraAlcance = true },
            { "speed", () => mejoras.mejoraVelocidad = true },
            { "bounce", () => mejoras.tieneRebote = true },
            { "t shoot", () => mejoras.tieneDisparoEnT = true },
            { "triple", () => mejoras.tieneDisparoTriple = true },
            { "homing", () => mejoras.tieneTeledirigido = true },
            { "machinegun", () => mejoras.tieneMetralleta = true }
        };
    }

    void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            IntentarCompra();
        }
    }

    void IntentarCompra()
    {
        if (mejoras == null || fichasControlador == null) return;

        if (TieneTodoAlMaximo())
        {
            Debug.Log("<color=orange>¡Ya tienes todo! No malgastes chapas.</color>");
            return;
        }

        if (fichasControlador.chapasActuales >= costeMejora)
        {
            CanjearAleatorio();
        }
        else
        {
            Debug.Log("Necesitas " + costeMejora + " fichas.");
        }
    }

    void CanjearAleatorio()
    {
        Sprite spriteElegido = null;
        string nombreProcesado = "";
        bool encontradaNueva = false;
        int intentos = 0;

        while (!encontradaNueva && intentos < 50)
        {
            float suerte = UnityEngine.Random.Range(0f, 100f);

            if (suerte <= 50f)
                spriteElegido = comunes[UnityEngine.Random.Range(0, comunes.Length)];
            else if (suerte <= 85f)
                spriteElegido = epicos[UnityEngine.Random.Range(0, epicos.Length)];
            else
                spriteElegido = legendarios[UnityEngine.Random.Range(0, legendarios.Length)];

            if (spriteElegido != null)
            {
                nombreProcesado = spriteElegido.name.ToLower().Trim();
                if (!YaTieneEstaMejora(nombreProcesado)) encontradaNueva = true;
            }
            intentos++;
        }

        if (encontradaNueva)
        {
            fichasControlador.chapasActuales -= costeMejora;
            fichasControlador.ActualizarUI();

            EjecutarMejora(nombreProcesado);
            LanzarIcono(spriteElegido);

            Debug.Log("<color=red>¡Cofre agotado!</color>");
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Debug.Log("Este cofre solo tenía cosas que ya tienes. ¡Busca otro!");
        }
    }

    bool YaTieneEstaMejora(string nombre)
    {
        foreach (var item in comprobadorMejoras)
        {
            if (nombre.Contains(item.Key)) return item.Value.Invoke();
        }
        return false;
    }

    void EjecutarMejora(string nombre)
    {
        foreach (var item in activadorMejoras)
        {
            if (nombre.Contains(item.Key))
            {
                item.Value.Invoke();
                return;
            }
        }
    }

    bool TieneTodoAlMaximo()
    {
        foreach (var item in comprobadorMejoras)
        {
            if (!item.Value.Invoke()) return false;
        }
        return true;
    }

    void LanzarIcono(Sprite s)
    {
        if (plantillaIcono != null)
        {
            GameObject icono = Instantiate(plantillaIcono, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            icono.GetComponent<SpriteRenderer>().sprite = s;
            if (!icono.GetComponent<ItemFlotante>()) icono.AddComponent<ItemFlotante>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) jugadorCerca = true; }
    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorCerca = false; }
}