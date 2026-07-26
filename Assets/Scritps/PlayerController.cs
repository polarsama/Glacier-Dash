using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float baseSpeed = 8f;
    public float currentSpeed;
    public float minSpeed = 3f;
    public float recoveryRate = 2f; // Velocidad con la que recupera la aceleracion perdida

    [Header("Dash Settings")]
    public float dashForce = 15f;
    public float dashDuration = 0.3f; // Tiempo que dura el estado de Dash activo
    
    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // 1. Manejo del tiempo del Dash
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false; // El Dash termina
            }
        }

        // 2. Recuperacion progresiva de velocidad tras un choque
        if (currentSpeed < baseSpeed)
        {
            currentSpeed += recoveryRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, baseSpeed);
        }

        // 3. Movimiento constante con la velocidad actual
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // 4. Deteccion de entrada
        bool pressSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool pressLeftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (pressSpace || pressLeftClick)
        {
            ExecuteDash();
        }
    }

    void ExecuteDash()
    {
        rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        isDashing = true;
        dashTimer = dashDuration; // Activa el estado de invulnerabilidad/rompe-obstaculos durante 0.3s
    }

    public void ApplySpeedPenalty(float penaltyAmount)
    {
        // Si esta haciendo Dash, no recibe penalizacion
        if (isDashing) return;

        currentSpeed = Mathf.Max(minSpeed, currentSpeed - penaltyAmount);
        Debug.Log("¡Chocaste! Velocidad reducida a: " + currentSpeed);
    }

    // Permite a la roca saber si el oso viene en Dash
    public bool IsDashing()
    {
        return isDashing;
    }
}