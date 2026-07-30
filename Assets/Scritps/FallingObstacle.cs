using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallSpeed = 15f;      // Velocidad a la que cae del cielo
    public float speedPenalty = 4f;    // Castigo de velocidad al chocar
    public float rockBonusPoints = 500f;// Puntos al romperlo con Dash

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // Reinicia la velocidad vertical al activarse desde el pool
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, -fallSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Colision con el suelo o limites de la escena -> se apaga
        if (collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
            return;
        }

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Si el jugador esta usando Dash o Modo Caña, destruye el objeto y gana puntos
            if (player.IsDashing() || player.IsCanaActive())
            {
                player.AddCanaEnergy(player.canaEnergyPerRock);
                player.AddBonusPoints(rockBonusPoints);
                gameObject.SetActive(false);
            }
            else
            {
                // Si le cae encima en estado normal, aplica penalizacion grave
                player.ApplySpeedPenalty(speedPenalty);
                gameObject.SetActive(false);
            }
        }
    }
}