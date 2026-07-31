using UnityEngine;

public class Avalanche : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform; // Posición del jugador
    public UIManager uiManager;       // Interfaz de usuario

    [Header("Rubber-banding Settings")]
    public float targetDistance = 8f;   // Distancia ideal constante que la avalancha busca mantener tras el jugador
    public float baseSpeed = 8f;        // Velocidad mínima de avance
    public float maxSpeed = 25f;        // Velocidad máxima para acortar distancia cuando el oso va muy lejos
    public float catchUpFactor = 1.5f;  // Que tan agresivamente acelera al quedar atras

    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseSpeed;

        // Auto-asignación de referencias si no están configuradas
        if (playerTransform == null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) playerTransform = player.transform;
        }

        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Calcular distancia actual entre el oso y la avalancha
        float currentDistance = playerTransform.position.x - transform.position.x;

        // Si el jugador está más lejos que la distancia objetivo, aumentar velocidad
        if (currentDistance > targetDistance)
        {
            // La velocidad escala proporcionalmente a la distancia de ventaja que lleva el jugador
            float excessDistance = currentDistance - targetDistance;
            currentSpeed = baseSpeed + (excessDistance * catchUpFactor);
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed); // Limitar a la velocidad máxima
        }
        else
        {
            // Si el jugador se retrasa y está muy cerca de la avalancha, esta vuelve a su velocidad base
            currentSpeed = baseSpeed;
        }

        // Movimiento lineal constante hacia la derecha
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);

        // Incrementa la velocidad base de la avalancha gradualmente con el tiempo
        baseSpeed += 0.05f * Time.deltaTime;
    }

    // Detección de colisión para Game Over
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Inmunidad durante el Modo Caña
            if (player.IsCanaActive()) return;

            // En estado normal ➔ Detener juego y mostrar Game Over
            if (uiManager != null)
            {
                uiManager.ShowGameOver(player.GetCurrentScore());
            }
        }
    }
}