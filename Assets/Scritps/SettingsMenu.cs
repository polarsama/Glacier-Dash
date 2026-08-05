using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioMixer masterAudioMixer;       
    public Slider masterVolumeSlider;         

    [Header("Main Settings Panel (Menu Principal de Ajustes)")]
    public GameObject mainSettingsPanel;    // El panel principal con los botones "Controls", "Credits", "Volume"

    [Header("Sub-Panels")]
    public GameObject controlsSubPanel;     // Panel para ver los controles / teclas

    void Start()
    {
        // Inicializar el valor del slider si existe
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = AudioListener.volume;
            masterVolumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Cambia el volumen global del juego
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; 
    }

    // Muestra la sección de controles y oculta el menú principal de ajustes
    public void ShowControls()
    {
        if (mainSettingsPanel != null) mainSettingsPanel.SetActive(false);
        if (controlsSubPanel != null) controlsSubPanel.SetActive(true);

    }



    // Botón "Back" dentro de Controles o Créditos: Regresa al menú principal de Ajustes
    public void BackToMainSettings()
    {
        if (mainSettingsPanel != null) mainSettingsPanel.SetActive(true);
        if (controlsSubPanel != null) controlsSubPanel.SetActive(false);

    }
}