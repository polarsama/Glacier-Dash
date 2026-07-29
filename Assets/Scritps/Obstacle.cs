using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speedPenalty = 3f; // Cantidad de velocidad que pierde el oso al chocar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Si el jugador esta usando Dash o esta en Modo Caña, destruye la roca y gana premios
            if (player.IsDashing() || player.IsCanaActive())
            {
                player.AddCanaEnergy(player.canaEnergyPerRock); // Suma barra de caña
                player.AddBonusPoints(player.rockBonusPoints);  // Suma bono de +500 PTS
                gameObject.SetActive(false);                   // Apaga la roca
            }
            else
            {
                // Si choca normal, frena al jugador y deshabilita el obstaculo
                player.ApplySpeedPenalty(speedPenalty);
                gameObject.SetActive(false);
            }
        }
    }
}