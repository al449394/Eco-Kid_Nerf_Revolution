using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ManejadorMuerte : MonoBehaviour
{
    private VisualElement _raiz;

    void OnEnable()
    {
        // Pillamos la referencia del UI Document
        _raiz = GetComponent<UIDocument>().rootVisualElement;

        // Buscamos los botones por los nombres
        Button btnRestart = _raiz.Q<Button>("btn-restart");
        Button btnMenu = _raiz.Q<Button>("btn-menu");

        // Conectamos el clic con sus funciones
        if (btnRestart != null)
        {
            btnRestart.clicked += ReiniciarNivel;
        }


        if (btnMenu != null)
        {
            btnMenu.clicked += IrAlMenu;
        }

        _raiz.style.display = DisplayStyle.None;
    }

    public void ActivarMenuMuerte()
    {
        Time.timeScale = 0f; // Pausa el juego
        UnityEngine.Cursor.lockState = CursorLockMode.None; // Libera el ratón
        UnityEngine.Cursor.visible = true; // Muestra el puntero
        _raiz.style.display = DisplayStyle.Flex;
    }

    void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Reseteamos el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaMenu");
    }
}