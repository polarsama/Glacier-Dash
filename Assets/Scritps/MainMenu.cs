using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Panel principal del menú de inicio (con el título y los botones Star, Settings, Quit)")]
    public GameObject mainButtonsPanel;

    [Tooltip("Panel de ajustes (el mismo que usas en el menú de pausa)")]
    public GameObject settingsPanel;

    [Header("Gameplay Scene Configuration")]
    [Tooltip("Nombre exacto de la escena de juego en Build Settings")]
    public string gameSceneName = "GameScene";

    private void Start()
    {
        // 1. Restablecemos el tiempo a su velocidad normal al cargar la pantalla de inicio
        Time.timeScale = 1f;

        // 2. SILENCIO TOTAL: Garantizamos que la música y cualquier efecto de audio se detengan por completo
        if (AudioManager.Instance != null)
        {
            // Llama a la función de detención en el AudioManager si este persiste entre escenas
            AudioManager.Instance.StopBackgroundMusic();

            // Limpia y detiene cualquier AudioSource adjunto al AudioManager
            AudioSource audioSource = AudioManager.Instance.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null; // Remueve el clip para evitar autoreproducción
            }
        }

        // 3. Estado inicial de la interfaz: botones principales visibles y panel de ajustes oculto
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Botón "Star": Carga e inicia la escena principal de juego
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Botón "Settings": Oculta la vista principal del título y abre el menú de ajustes
    public void OpenSettings()
    {
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Botón "Back" dentro de Settings: Cierra ajustes y vuelve a los botones principales del título
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(true);
    }

    // Botón "Quit": Cierra la aplicación/ejecutable del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo de Glacier Dash...");
        Application.Quit();
    }
}