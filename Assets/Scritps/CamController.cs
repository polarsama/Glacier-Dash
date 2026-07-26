using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform objectToFollow; // Aquí arrastraremos al Jugador 
    public Vector3 offset = new Vector3(3f, 1f, -10f); // Desplazamiento para que el oso no esté en el centro exacto, sino un poco a la izquierda
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (objectToFollow == null) return;

        // Posición a la que queremos que vaya la cámara (siguiendo al jugador en X e Y, manteniendo Z)
        Vector3 targetPosition = new Vector3(objectToFollow.position.x + offset.x, objectToFollow.position.y + offset.y, offset.z);

        // Mover la cámara suavemente hacia esa posición usando Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}