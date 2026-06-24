using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class Pausa : MonoBehaviour
{
    private VisualElement _raiz;
    private VisualElement _pestana;
    private bool _pausado = false;

    void OnEnable()
    {
        // referencia al documento UI
        var doc = GetComponent<UIDocument>();
        _raiz = doc.rootVisualElement;

        // buscar la pestaña
        _pestana = _raiz.Q<VisualElement>("panel-pestana");

        // configurar los clics de los botones
        _raiz.Q<Button>("btn-resume").clicked += () => StartCoroutine(QuitarPausa());
        _raiz.Q<Button>("btn-restart").clicked += Reiniciar;
        _raiz.Q<Button>("btn-menu").clicked += IrAlMenu;

        // empezar con el menú oculto
        _raiz.style.display = DisplayStyle.None;
    }

    void Update()
    {
        // detectar la tecla Escap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pausado) PonerPausa();
            else StartCoroutine(QuitarPausa());
        }
    }

    void PonerPausa()
    {
        _pausado = true;
        Time.timeScale = 0f; // congela el tiempo del juego

        _raiz.style.display = DisplayStyle.Flex; // muestra la interfaz
        _pestana.AddToClassList("visible");   

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator QuitarPausa()
    {
        _pausado = false;
        _pestana.RemoveFromClassList("visible");

        // Esperamos 0.5 segundos reales (porque el tiempo del juego está a 0)
        yield return new WaitForSecondsRealtime(0.5f);

        _raiz.style.display = DisplayStyle.None; 
        Time.timeScale = 1f;                     

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    void Reiniciar()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaMenu");
    }
}