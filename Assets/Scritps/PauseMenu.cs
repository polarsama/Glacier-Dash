using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Panel principal del menú de pausa")]
    public GameObject pauseMenuUI;

    [Tooltip("Panel secundario de ajustes")]
    public GameObject settingsPanel;

    private bool isPaused = false;
    private InputAction cancelAction;

    private void Awake()
    {
        // Creamos una acción de entrada directa ligada a la tecla Escape
        // Esto evita que el EventSystem del nuevo Input System bloquee la lectura de teclado
        cancelAction = new InputAction(binding: "<Keyboard>/escape");
        cancelAction.performed += ctx => TogglePause();
    }

    private void OnEnable()
    {
        // Activamos la lectura de la tecla cuando el objeto se habilita en la escena
        cancelAction.Enable();
    }

    private void OnDisable()
    {
        // Desactivamos la lectura al deshabilitar el objeto para liberar memoria
        cancelAction.Disable();
    }

    // Alterna el estado del juego al presionar la tecla Escape
    public void TogglePause()
    {
        // Si el submenú de ajustes está abierto, Escape actúa como botón "Atrás" para volver a la pausa
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            CloseSettings();
            return;
        }

        // Si el menú de ajustes no está abierto, alterna entre pausar y reanudar el juego
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Congela el tiempo del juego y despliega únicamente el panel principal de pausa
    public void PauseGame()
    {
        // Aseguramos que ajustes esté cerrado al pausar
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // Encendemos la interfaz del menú de pausa
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f; // Congela el tiempo del juego (físicas, movimiento, animaciones basadas en delta time)
        isPaused = true;
    }

    // Despausa el juego y oculta todos los paneles de la interfaz
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Cierra el panel de ajustes si estaba abierto
        }

        Time.timeScale = 1f; // Devuelve la escala del tiempo a la velocidad normal (1.0)
        isPaused = false;
    }

    // Oculta el menú de pausa principal y abre el submenú de ajustes
    public void OpenSettings()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Apaga la UI de pausa para evitar solapamiento
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Muestra el panel de ajustes
        }
    }

    // Cierra el submenú de ajustes y restablece la vista del menú de pausa principal
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Oculta ajustes
        }
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Vuelve a mostrar el menú de pausa principal
        }
    }

    // Restablece el tiempo y regresa a la pantalla del menú principal / título
    public void GoToTitleScreen(string titleSceneName = "Title")
    {
        Time.timeScale = 1f; // Es crucial restablecer el tiempo antes de cambiar de escena

        // Detener la música de fondo al salir al menú principal (si existe el AudioManager)
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBackgroundMusic();
        }

        SceneManager.LoadScene(titleSceneName);
    }

    // Cierra por completo la aplicación del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}