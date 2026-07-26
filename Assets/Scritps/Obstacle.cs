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
            if (player.IsDashing())
            {
                Debug.Log("¡ROCA DESTRUIDA CON DASH!");
                // Aqui podremos agregar mas adelante un efecto visual o sonido de destruccion
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