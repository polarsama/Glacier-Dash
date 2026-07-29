using UnityEngine;

public class Avalanche : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform; // Referencia a la posicion del jugador
    public UIManager uiManager;       // Referencia al gestor de interfaz

    [Header("Movement Settings")]
    public float baseAvalancheSpeed = 7.5f; // Velocidad base de avance de la avalancha
    public float maxDistanceBehind = 15f;    // Distancia maxima a la que puede quedarse atras
    public float minDistanceBehind = 3f;     // Distancia minima antes de considerarse peligro crítico

    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseAvalancheSpeed;

        // Auto-asignacion del Player si no se coloco en Inspector
        if (playerTransform == null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) playerTransform = player.transform;
        }

        // Auto-asignacion del UIManager si no se coloco
        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Calculo de la distancia horizontal respecto al jugador
        float distanceToPlayer = playerTransform.position.x - transform.position.x;

        // Ajuste dinamico de velocidad: si el oso se aleja demasiado, la avalancha acelera para mantener la presion
        if (distanceToPlayer > maxDistanceBehind)
        {
            currentSpeed = baseAvalancheSpeed * 1.3f;
        }
        else
        {
            currentSpeed = baseAvalancheSpeed;
        }

        // Movimiento constante de la avalancha hacia la derecha
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
    }

    // Detecta la colision directa con el jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Si el jugador esta en Modo Caña, la avalancha no lo elimina (lo impulsa o es inmune)
            if (player.IsCanaActive()) return;

            // Si el jugador es alcanzado en estado normal ➔ Game Over
            if (uiManager != null)
            {
                uiManager.ShowGameOver(player.GetCurrentScore());
            }
        }
    }
}