using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public float speedPenalty = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.IsDashing() || player.IsCanaActive())
            {
                // Rompe la roca y le da carga a la Barra de Caña
                player.AddCanaEnergy(player.canaEnergyPerRock);
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