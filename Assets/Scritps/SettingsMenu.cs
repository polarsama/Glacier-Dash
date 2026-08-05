using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("Main Settings View")]
    public GameObject mainSettingsButtons; // El contenedor con los botones "Controls", "Credits" y "Back to Pause"

    [Header("Sub-Panels")]
    public GameObject controlsSubPanel;     // Panel visual de Controles
    public GameObject creditsSubPanel;      // Panel visual de Créditos

    private void OnEnable()
    {
        // Cada vez que se active este panel, asegura mostrar el menú principal de Ajustes y ocultar subpaneles
        OpenMainSettings();
    }

    public void OpenMainSettings()
    {
        if (mainSettingsButtons != null) mainSettingsButtons.SetActive(true);
        if (controlsSubPanel != null) controlsSubPanel.SetActive(false);
        if (creditsSubPanel != null) creditsSubPanel.SetActive(false);
    }

    public void ShowControls()
    {
        if (mainSettingsButtons != null) mainSettingsButtons.SetActive(false);
        if (controlsSubPanel != null) controlsSubPanel.SetActive(true);
        if (creditsSubPanel != null) creditsSubPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        if (mainSettingsButtons != null) mainSettingsButtons.SetActive(false);
        if (controlsSubPanel != null) controlsSubPanel.SetActive(false);
        if (creditsSubPanel != null) creditsSubPanel.SetActive(true);
    }

    // Usar este método exclusivamente en los botones "Back" DENTRO de Controles y Créditos
    public void BackToMainSettings()
    {
        OpenMainSettings();
    }
}