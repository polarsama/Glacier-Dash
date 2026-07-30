using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallSpeed = 12f;       // Velocidad a la que cae del cielo
    public float speedPenalty = 8f;     // Castigo de velocidad al impactar al oso
    public float rockBonusPoints = 500f;// Puntos al romperlo con Dash

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Da velocidad de caida vertical inmediatamente
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, -fallSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Busca si el objeto con el que choco es el Player Controller
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Caso A: El jugador esta en Dash o Modo Caña -> destruye el hielo y da premio
            if (player.IsDashing() || player.IsCanaActive())
            {
                Debug.Log("¡Destruiste el hielo cayendo con Dash!");
                player.AddCanaEnergy(player.canaEnergyPerRock);
                player.AddBonusPoints(rockBonusPoints);
                Destroy(gameObject);
            }
            // Caso B: El jugador esta en estado normal -> recibe el golpe y pierde velocidad
            else
            {
                Debug.Log("¡El hielo te golpeo! Aplicando castigo de velocidad.");
                player.ApplySpeedPenalty(speedPenalty);
                Destroy(gameObject);
            }
            return;
        }

        // Si choca con el suelo (Tag: Ground), se destruye sin hacer nada
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}