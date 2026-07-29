using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Requerido para reiniciar la escena

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI scoreText;      // Texto de puntos actuales
    public TextMeshProUGUI highScoreText;  // Texto de record historico
    public TextMeshProUGUI speedText;      // Texto de velocidad actual

    [Header("Cana Mode UI References")]
    public Slider canaBarSlider;           // Slider de la barra de caña
    public Image canaBarFill;              // Relleno de la barra
    public Color normalColor = Color.yellow; 
    public Color fullColor = Color.cyan;     

    [Header("Game Over UI References")]
    public GameObject gameOverPanel;       // Panel contenedor de Game Over
    public TextMeshProUGUI finalScoreText; // Texto que muestra los puntos finales logrados

    // Muestra la puntuacion actual
    public void UpdateScore(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = "PTS: " + Mathf.FloorToInt(score).ToString();
        }
    }

    // Muestra el record historico
    public void UpdateHighScore(int highScore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = "BEST: " + highScore.ToString();
        }
    }

    // Muestra la velocidad actual
    public void UpdateSpeed(float speed)
    {
        if (speedText != null)
        {
            speedText.text = "Velocidad: " + speed.ToString("F1") + " km/h";
        }
    }

    // Actualiza la barra del Modo Caña
    public void UpdateCanaBar(float currentAmount, float maxAmount, bool isCanaActive)
    {
        if (canaBarSlider != null)
        {
            canaBarSlider.maxValue = maxAmount;
            canaBarSlider.value = currentAmount;

            if (canaBarFill != null)
            {
                canaBarFill.color = isCanaActive ? fullColor : normalColor;
            }
        }
    }

    // Muestra la pantalla de Game Over y congela la partida
    public void ShowGameOver(float finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Enciende el panel
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Game Over: " +"\n" + "Final Score: " + Mathf.FloorToInt(finalScore).ToString();
        }

        Time.timeScale = 0f; // Detiene el tiempo del juego totalmente
    }

    // Metodo para reiniciar la partida (conectado al boton de reinicio)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reestablece la velocidad del tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }
}