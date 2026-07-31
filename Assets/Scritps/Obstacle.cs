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
                Debug.Log("¡Golpeaste una Roca de Espinas! No se destruye con Dash.");
                player.ApplySpeedPenalty(speedPenalty);
                return;
            }

            // SI ES ROCA NORMAL:
            if (player.IsDashing() || player.IsCanaActive())
            {
                player.AddCanaEnergy(player.canaEnergyPerRock);
                player.AddBonusPoints(500f);
                gameObject.SetActive(false);
            }
            else
            {
                player.ApplySpeedPenalty(speedPenalty);
                gameObject.SetActive(false);
            }
        }
    }
}