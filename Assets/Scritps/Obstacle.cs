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

                // Forzamos la penalización de velocidad en el jugador
                player.ApplyDirectSpeedPenalty(speedPenalty);

                // Disparamos el temblor de cámara
                CamController cam = Camera.main.GetComponent<CamController>();
                if (cam != null)
                {
                    cam.TriggerShake(0.3f, 0.45f);
                }

                // EFECTO DE SONIDO: Reproduce el golpe de penalización al impactar las espinas
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.penaltySFX);
                }

                return; 
            }

            // SI ES ROCA NORMAL:
            if (player.IsDashing() || player.IsCanaActive())
            {
                player.AddCanaEnergy(player.canaEnergyPerRock);
                player.AddBonusPoints(500f);

                // Efecto de sonido: Romper roca con Dash o Modo Caña
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.rockBreakSFX);
                }

                gameObject.SetActive(false); 
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