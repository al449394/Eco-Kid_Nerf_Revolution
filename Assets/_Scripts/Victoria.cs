using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    private VisualElement _raiz;

    void OnEnable()
    {
        // obtenemos el documento de UI
        _raiz = GetComponent<UIDocument>().rootVisualElement;

        // buscamos los botones por su nombre en UI Builder
        Button btnRestart = _raiz.Q<Button>("btn-restart");
        Button btnMenu = _raiz.Q<Button>("btn-menu");

        // conectamos los clics con las funciones
        if (btnRestart != null)
        {
            btnRestart.clicked += ReiniciarJuego;
            Debug.Log("Botón reiniciar de victoria detectado.");
        }

        if (btnMenu != null)
        {
            btnMenu.clicked += IrAlMenu;
            Debug.Log("Botón menú de victoria detectado.");
        }
    }

    void ReiniciarJuego()
    {
        Debug.Log("Reiniciando el juego...");
        Time.timeScale = 1f; 
        SceneManager.LoadScene("SampleScene");
    }

    void IrAlMenu()
    {
        SceneManager.LoadScene("EscenaMenu");
    }
}