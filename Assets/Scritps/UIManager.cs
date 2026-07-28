using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;

    [Header("Cana Mode UI References")]
    public Slider canaBarSlider;
    public Image canaBarFill;
    public Color normalColor = Color.yellow;
    public Color fullColor = Color.cyan;

    // Actualiza la distancia/puntuacion en pantalla
    public void UpdateScore(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(score).ToString() + " m";
        }
    }

    // Actualiza el medidor de velocidad
    public void UpdateSpeed(float speed)
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + speed.ToString("F1");
        }
    }

    // Actualiza la barra del Modo Caña
    public void UpdateCanaBar(float currentAmount, float maxAmount, bool isCanaActive)
    {
        if (canaBarSlider != null)
        {
            canaBarSlider.maxValue = maxAmount;
            canaBarSlider.value = currentAmount;

            // Cambia el color de la barra cuando el Modo Caña esta listo o activo
            if (canaBarFill != null)
            {
                canaBarFill.color = isCanaActive ? fullColor : normalColor;
            }
        }
    }
}