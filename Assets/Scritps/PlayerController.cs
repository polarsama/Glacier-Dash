using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public UIManager uiManager;

    [Header("Speed Settings")]
    public float baseSpeed = 8f;
    public float currentSpeed;
    public float minSpeed = 3f;
    public float recoveryRate = 2f; // Velocidad con la que recupera la aceleracion perdida

    [Header("Dash Settings")]
    public float dashForce = 15f;
    public float dashDuration = 0.3f; // Tiempo que dura el estado de Dash activo

    private bool isDashing = false;

    private float dashTimer = 0f;
    
    [Header("Modo Caña Settings")]
    public float currentCanaEnergy = 0f;
    public float maxCanaEnergy = 100f;
    public float canaEnergyPerRock = 25f; // Energia que otorga romper una roca
    public float canaDuration = 5f;       // Duracion del Modo Caña activo
    private bool isCanaActive = false;
    private float canaTimer = 0f;

    private Rigidbody2D rb;
    private float score = 0f;
    private float startX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
        startX = transform.position.x; // Establecemos el punto de inicio
    }

    void Update()
    {
       // 1. Calculo de Puntuacion basada en distancia recorrida
        score = transform.position.x - startX;

        // 2. Manejo del tiempo del Dash
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f) isDashing = false;
        }

        // 3. Manejo del Modo Caña
        if (isCanaActive)
        {
            canaTimer -= Time.deltaTime;
            if (canaTimer <= 0f)
            {
                DeactivateCanaMode();
            }
        }
        else if (currentSpeed < baseSpeed)
        {
            // Recuperacion normal de velocidad solo si no esta en Modo Caña
            currentSpeed += recoveryRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, baseSpeed);
        }

        // Movimiento constante
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // Controles
        bool pressSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool pressLeftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (pressSpace || pressLeftClick)
        {
            ExecuteDash();
        }

        // 4. Actualizar Interfaz de Usuario
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
            uiManager.UpdateSpeed(currentSpeed * 2.5f); // Multiplicador estetico para mostrar km/h
            uiManager.UpdateCanaBar(isCanaActive ? canaTimer : currentCanaEnergy, isCanaActive ? canaDuration : maxCanaEnergy, isCanaActive);
        }
    }

    void ExecuteDash()
    {
        rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        isDashing = true;
        dashTimer = dashDuration; // Activa el estado de invulnerabilidad/rompe-obstaculos durante 0.3s
    }

    public void AddCanaEnergy(float amount)
    {
        if (isCanaActive) return;

        currentCanaEnergy += amount;
        if (currentCanaEnergy >= maxCanaEnergy)
        {
            ActivateCanaMode();
        }
    }

    void ActivateCanaMode()
    {
        isCanaActive = true;
        canaTimer = canaDuration;
        currentCanaEnergy = 0f;
        currentSpeed = baseSpeed * 1.8f; // Super velocidad
        Debug.Log("¡MODO CAÑA ACTIVADO!");
    }

    void DeactivateCanaMode()
    {
        isCanaActive = false;
        currentSpeed = baseSpeed;
        Debug.Log("Modo Caña finalizado");
    }

    public void ApplySpeedPenalty(float penaltyAmount)
    {
        // En Modo Caña o durante un Dash, es invulnerable
        if (isDashing || isCanaActive) return;

        currentSpeed = Mathf.Max(minSpeed, currentSpeed - penaltyAmount);
    }

    public bool IsDashing() => isDashing;
    public bool IsCanaActive() => isCanaActive;
}