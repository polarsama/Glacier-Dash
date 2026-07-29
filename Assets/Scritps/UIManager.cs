using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI scoreText;      // Texto para los puntos de la partida actual
    public TextMeshProUGUI highScoreText;  // Texto para el record maximo
    public TextMeshProUGUI speedText;      // Texto para la velocidad actual

    [Header("Cana Mode UI References")]
    public Slider canaBarSlider;           // Componente Slider de la barra
    public Image canaBarFill;              // Relleno visual de la barra de caña
    public Color normalColor = Color.yellow; // Color base de la barra
    public Color fullColor = Color.cyan;     // Color de la barra cuando esta en Modo Caña

    // Muestra la puntuacion actual en formato de puntos (PTS)
    public void UpdateScore(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }

    // Muestra el record historico guardado
    public void UpdateHighScore(int highScore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    // Muestra la velocidad convertida a formato km/h
    public void UpdateSpeed(float speed)
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + speed.ToString("F1");
        }
    }

    // Actualiza el progreso y color de la barra del Modo Caña
    public void UpdateCanaBar(float currentAmount, float maxAmount, bool isCanaActive)
    {
        if (canaBarSlider != null)
        {
            canaBarSlider.maxValue = maxAmount;
            canaBarSlider.value = currentAmount;

            // Cambia el color del Fill si el modo super velocidad esta activo
            if (canaBarFill != null)
            {
                canaBarFill.color = isCanaActive ? fullColor : normalColor;
            }
        }
    }
}