using UnityEngine;

public class MejorasJugador : MonoBehaviour
{
    [Header("Mejoras de la Bala")]
    //mejoras comunes
    public bool mejoraDanio = false;
    public bool mejoraAlcance = false;
    public bool mejoraCadencia = false;

    //mejoras epicas
    public bool tieneRebote = false;
    public bool tieneDisparoEnT = false;
    public bool tieneDisparoTriple = false;

    //mejoras legendarias
    public bool tieneMetralleta = false;
    public bool tieneTeledirigido = false; 

    [Header("Mejoras de Personaje")]
    public bool mejoraVelocidad = false;
}