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

    [Header("Jump Settings")]
    public float jumpForce = 12f;          // Fuerza aplicada al saltar
    public Transform groundCheck;          // Objeto hijo ubicado en los pies del personaje
    public float groundCheckRadius = 0.2f; // Radio de la esfera de detección de suelo
    public LayerMask groundLayer;          // Capa asignada a las plataformas de suelo
    private int jumpCount = 0;             // Contador interno de saltos realizados
    public int maxJumps = 2;               // Límite máximo para permitir Doble Salto
    private bool isGrounded;               // Bandera que indica si el personaje toca el suelo

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

    [Header("Difficulty Progression Settings")]
    public float speedIncreaseRate = 0.1f; // Cantidad de velocidad que aumenta por segundo de juego
    public float maxBaseSpeed = 20f;       // Velocidad base máxima a la que se puede llegar

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
        // Verificación de contacto con el suelo
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // Si toca el suelo, se reinicia el contador de saltos
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Incremento progresivo de la dificultad (Velocidad Base escala con el tiempo)
        if (baseSpeed < maxBaseSpeed)
        {
            baseSpeed += speedIncreaseRate * Time.deltaTime;
            baseSpeed = Mathf.Min(baseSpeed, maxBaseSpeed);
        }

        // Acumulación progresiva de puntos por tiempo
        currentScore += pointsPerSecond * Time.deltaTime;
        int currentScoreInt = Mathf.FloorToInt(currentScore);

        // Verificación y guardado automático de récord
        if (currentScoreInt > highScore)
        {
            highScore = currentScoreInt;
            SaveSystem.SaveHighScore(highScore);
            if (uiManager != null) uiManager.UpdateHighScore(highScore);
        }

        // Temporizador de Dash
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f) isDashing = false;
        }

        // Temporizador de Modo Caña y recuperación de velocidad
        if (isCanaActive)
        {
            canaTimer -= Time.deltaTime;
            if (canaTimer <= 0f) DeactivateCanaMode();
        }
        else if (currentSpeed < baseSpeed)
        {
            // La velocidad recupera el valor de baseSpeed (que ahora va subiendo con la dificultad)
            currentSpeed += recoveryRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, baseSpeed);
        }
        else if (!isDashing)
        {
            // Mantiene la velocidad actual sincronizada con la nueva baseSpeed alcanzada
            currentSpeed = baseSpeed;
        }

        // Aplicación del movimiento horizontal
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // Controles de entrada
        bool pressSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool pressLeftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        // Espacio para SALTO / DOBLE SALTO
        if (pressSpace)
        {
            ExecuteJump();
        }

        // Clic Izquierdo para DASH
        if (pressLeftClick && !isDashing)
        {
            ExecuteDash();
        }

        // Actualización de la interfaz gráfica
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
            uiManager.UpdateSpeed(currentSpeed * 2.5f);
            uiManager.UpdateCanaBar(isCanaActive ? canaTimer : currentCanaEnergy, isCanaActive ? canaDuration : maxCanaEnergy, isCanaActive);
        }
    }

    // Ejecuta el impulso del Salto / Doble Salto
    void ExecuteJump()
    {
        if (jumpCount < maxJumps)
        {
            // Resetea la velocidad vertical para garantizar la misma altura en el segundo salto
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
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

        // RESET DE SALTO: Al chocar con espinas en el suelo, permitimos que pueda saltar de inmediato
        jumpCount = 0;

        // EFECTO VISUAL: Sacudida de cámara por impacto y penalización de velocidad
        CamController cam = Camera.main.GetComponent<CamController>();
        if (cam != null)
        {
            cam.TriggerShake(0.25f, 0.35f); // 0.25 segundos de duración con una intensidad de 0.35
        }
    }

    // Metodos publicos para consultar estados desde otros scripts
    public bool IsDashing() => isDashing;
    public bool IsCanaActive() => isCanaActive;

    // Metodo publico para obtener el puntaje actual en tiempo real
    public float GetCurrentScore()
    {
        return currentScore;
    }

    // Dibujado visual del gizmo en el editor para verificar la zona de pies
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}