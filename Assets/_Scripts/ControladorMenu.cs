using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    private UIDocument _documento;
    private Button _botonPlay;
    private Button _botonExit;

    void OnEnable()
    {
        _documento = GetComponent<UIDocument>();

        // Busca los botones por ID
        _botonPlay = _documento.rootVisualElement.Q<Button>("boton-play");
        _botonExit = _documento.rootVisualElement.Q<Button>("boton-exit");

        // Asigna las funciones a los clics
        if (_botonPlay != null) _botonPlay.clicked += IniciarJuego;
        if (_botonExit != null) _botonExit.clicked += SalirJuego;
    }

    void IniciarJuego()
    {
        // Carga la escena principal
        SceneManager.LoadScene("SampleScene");
    }

    void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");

        // Esto detiene el modo Play dentro de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}