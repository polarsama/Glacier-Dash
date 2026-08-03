using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public bool isSpikeObstacle = false; // [X] Marca esta casilla en el Prefab de la roca de espinas
    public float speedPenalty = 3.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // SI ES ROCA DE ESPINAS / TRAMPA:
            if (isSpikeObstacle)
            {
                Debug.Log("¡Golpeaste una Roca de Espinas! Es indestructible.");

                // Si está en Modo Caña (invencible total), ignora las espinas
                if (player.IsCanaActive()) return;

                // Forzamos la penalización de velocidad (incluso si está en Dash)
                player.ApplyDirectSpeedPenalty(speedPenalty);

                // Disparamos el temblor de cámara directamente
                CamController cam = Camera.main.GetComponent<CamController>();
                if (cam != null)
                {
                    cam.TriggerShake(0.3f, 0.45f); // 0.3 segundos de duración con buena intensidad
                }

                return; 
            }

            // SI ES ROCA NORMAL:
            if (player.IsDashing() || player.IsCanaActive())
            {
                player.AddCanaEnergy(player.canaEnergyPerRock);
                player.AddBonusPoints(500f);
                gameObject.SetActive(false); // Esta SI se destruye con Dash
            }
            else
            {
                player.ApplySpeedPenalty(speedPenalty);

                // Temblor ligero al chocar contra roca normal sin Dash
                CamController cam = Camera.main.GetComponent<CamController>();
                if (cam != null)
                {
                    cam.TriggerShake(0.2f, 0.3f);
                }

                gameObject.SetActive(false);
            }
        }
    }
}