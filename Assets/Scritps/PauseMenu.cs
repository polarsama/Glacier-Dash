using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuUI;    // El panel principal del menú de pausa
    public GameObject settingsPanel;  // El panel de ajustes (opcional)

    private bool isPaused = false;

    void Update()
    {
        // Detecta si se presiona la tecla Escape para pausar o despausar
        bool pressEscape = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;

        if (pressEscape)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pausa el juego
    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f; // Congela el tiempo del juego
        isPaused = true;

        // Opcional: Pausar o bajar volumen de la música si lo deseas
        // if (AudioManager.Instance != null) { /* Opcional */ }
    }

    // Despausa el juego (Opción: Volver al juego)
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Cierra ajustes si estaba abierto
        }

        Time.timeScale = 1f; // Reanuda el tiempo normal
        isPaused = false;
    }

    // Abre el submenú de ajustes
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    // Cierra ajustes y vuelve al menú de pausa principal
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
    }

    // Vuelve a la pantalla de título / menú principal
    public void GoToTitleScreen(string titleSceneName = "TitleScene")
    {
        Time.timeScale = 1f; // Asegurar que el tiempo corra al cambiar de escena

        // Detener la música de fondo al salir al menú principal
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBackgroundMusic();
        }

        SceneManager.LoadScene(titleSceneName);
    }

    // Cierra la aplicación por completo
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}