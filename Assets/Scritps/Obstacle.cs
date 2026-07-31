using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Type")]
    public bool isSpikeObstacle = false; // [X] Si se marca, NO se puede romper con Dash

    [Header("Settings")]
    public float speedPenalty = 3.5f;
    public float bounceForceX = -4f;     // Fuerza que empuja al jugador hacia atrás al golpear la roca de espinas

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // --- ROCA DE ESPINAS / NO-DASH ---
            if (isSpikeObstacle)
            {
                // Si intenta usar Dash o Modo Caña contra espinas, rebota y recibe penalización
                Debug.Log("¡Chocaste contra una Roca de Espinas! No se puede romper con Dash.");
                
                // Aplicar penalización de velocidad
                player.ApplySpeedPenalty(speedPenalty);
                
                // Empujar un poco al jugador hacia la izquierda (hacia la avalancha)
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.AddForce(new Vector2(bounceForceX, 2f), ForceMode2D.Impulse);
                }

            }

            // --- ROCA NORMAL ---
            if (player.IsDashing() || player.IsCanaActive())
            {
                player.AddCanaEnergy(player.canaEnergyPerRock);
                player.AddBonusPoints(500f);
                gameObject.SetActive(false); // O Destroy(gameObject)
            }
            else
            {
                player.ApplySpeedPenalty(speedPenalty);
                gameObject.SetActive(false);
            }
        }
    }
}