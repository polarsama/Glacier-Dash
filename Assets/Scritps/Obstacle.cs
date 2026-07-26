using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speedPenalty = 3f;

    // DEBE decir OnTriggerEnter2D (con D al final)
    // y el parametro DEBE ser Collider2D (con 2D al final)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("¡CONTACTO REGISTRADO CON: " + collision.gameObject.name + "!");

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ApplySpeedPenalty(speedPenalty);
            gameObject.SetActive(false);
        }
    }
}