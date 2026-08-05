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
        // Aseguramos que el tiempo corra a velocidad normal al estar en el menú principal
        Time.timeScale = 1f;

        // Aseguramos que los botones principales estén visibles y los ajustes ocultos
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Botón "Star": Inicia la escena del juego
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Botón "Settings": Abre el panel de ajustes en la pantalla principal
    public void OpenSettings()
    {
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Botón "Back" dentro de Settings: Cierra ajustes y vuelve a los botones principales
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(true);
    }

    // Botón "Quit": Cierra la aplicación por completo
    public void QuitGame()
    {
        Debug.Log("Saliendo de Glacier Dash...");
        Application.Quit();
    }
}