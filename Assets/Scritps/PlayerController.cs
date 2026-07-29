using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public UIManager uiManager; // Referencia al gestor de la interfaz de usuario

    [Header("Speed Settings")]
    public float baseSpeed = 8f;         // Velocidad normal de carrera
    public float currentSpeed;           // Velocidad actual del jugador
    public float minSpeed = 3f;          // Velocidad minima tras un choque
    public float recoveryRate = 2f;      // Tasa de recuperacion de velocidad por segundo

    [Header("Dash Settings")]
    public float dashForce = 15f;        // Impulso de fuerza al presionar Dash
    public float dashDuration = 0.3f;    // Duracion del estado de invulnerabilidad por Dash
    private bool isDashing = false;      // Bandera que indica si esta usando Dash
    private float dashTimer = 0f;        // Temporizador interno del Dash

    [Header("Modo Caña Settings")]
    public float currentCanaEnergy = 0f; // Carga actual de la barra de caña
    public float maxCanaEnergy = 100f;   // Carga requerida para activar el modo
    public float canaEnergyPerRock = 25f;// Carga obtenida por romper una roca
    public float canaDuration = 5f;      // Duracion del Modo Caña activo
    private bool isCanaActive = false;   // Bandera que indica si esta en Modo Caña
    private float canaTimer = 0f;        // Temporizador de duracion del modo

    [Header("Score Settings")]
    public float pointsPerSecond = 100f;     // Puntos base ganados por segundo sobreviviendo
    public float rockBonusPoints = 500f;     // Puntos extra por romper rocas con Dash
    public float canaActivationBonus = 1000f;// Puntos extra por activar el Modo Caña
    private float currentScore = 0f;         // Contador de puntos acumulados en la partida
    private int highScore = 0;               // Valor del record guardado

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;

        // Auto-asignacion del UIManager si no se arrastro manualmente
        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>(); 
        }       
        // Carga del record previo guardado desde el SaveSystem
        highScore = SaveSystem.LoadHighScore();

        // Envia el record cargado a la interfaz
        if (uiManager != null)
        {
            uiManager.UpdateHighScore(highScore);
        }
    }

    void Update()
    {
        // 1. Acumulacion progresiva de puntos por tiempo
        currentScore += pointsPerSecond * Time.deltaTime;
        int currentScoreInt = Mathf.FloorToInt(currentScore);

        // 2. Verificacion y guardado automatico en tiempo real si se supera el record
        if (currentScoreInt > highScore)
        {
            highScore = currentScoreInt;
            SaveSystem.SaveHighScore(highScore); // Se escribe en disco
            if (uiManager != null) uiManager.UpdateHighScore(highScore);
        }

        // 3. Temporizador y estado de Dash
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f) isDashing = false;
        }

        // 4. Temporizador y estado del Modo Caña
        if (isCanaActive)
        {
            canaTimer -= Time.deltaTime;
            if (canaTimer <= 0f) DeactivateCanaMode();
        }
        else if (currentSpeed < baseSpeed)
        {
            // Recuperacion suave de velocidad tras frenar
            currentSpeed += recoveryRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, baseSpeed);
        }

        // Aplicacion de velocidad lineal horizontal
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // Controles de entrada (Espacio o Clic Izquierdo)
        bool pressSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool pressLeftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (pressSpace || pressLeftClick) ExecuteDash();

        // Actualizacion frame a frame de la UI
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
            uiManager.UpdateSpeed(currentSpeed * 2.5f); // Factor de conversion estético a km/h
            uiManager.UpdateCanaBar(isCanaActive ? canaTimer : currentCanaEnergy, isCanaActive ? canaDuration : maxCanaEnergy, isCanaActive);
        }
    }

    // Ejecuta el impulso del Dash
    void ExecuteDash()
    {
        rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        isDashing = true;
        dashTimer = dashDuration;
    }

    // Otorga energia a la barra del Modo Caña
    public void AddCanaEnergy(float amount)
    {
        if (isCanaActive) return; // No acumula mas mientras el modo esta encendido

        currentCanaEnergy += amount;
        if (currentCanaEnergy >= maxCanaEnergy)
        {
            ActivateCanaMode();
        }
    }

    // Otorga bonos extra de puntos de forma instantanea
    public void AddBonusPoints(float amount)
    {
        currentScore += amount;
    }

    // Activa el Modo Caña con super velocidad e invencibilidad
    void ActivateCanaMode()
    {
        isCanaActive = true;
        canaTimer = canaDuration;
        currentCanaEnergy = 0f;
        currentSpeed = baseSpeed * 1.8f; // Aumento de velocidad
        AddBonusPoints(canaActivationBonus); // Bono especial de puntos
    }

    // Desactiva el Modo Caña y vuelve a velocidad normal
    void DeactivateCanaMode()
    {
        isCanaActive = false;
        currentSpeed = baseSpeed;
    }

    // Aplica frenado al chocar contra una roca
    public void ApplySpeedPenalty(float penaltyAmount)
    {
        // Si esta en Dash o Modo Caña, no recibe castigo
        if (isDashing || isCanaActive) return;

        currentSpeed = Mathf.Max(minSpeed, currentSpeed - penaltyAmount);
    }

    // Metodos publicos para consultar estados desde otros scripts
    public bool IsDashing() => isDashing;
    public bool IsCanaActive() => isCanaActive;

    // Metodo publico para obtener el puntaje actual en tiempo real
    public float GetCurrentScore()
    {
        return currentScore;
    }
}