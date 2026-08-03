using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform objectToFollow; // Aquí arrastraremos al Jugador 
    public Vector3 offset = new Vector3(3f, 1f, -10f); // Desplazamiento para que el oso no esté en el centro exacto, sino un poco a la izquierda
    public float smoothSpeed = 5f;

    [Header("Camera Shake Settings")]
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.35f;
    private float dampingSpeed = 1.0f;
    private Vector3 shakeOffset = Vector3.zero;

    void LateUpdate()
    {
        if (objectToFollow == null) return;

        // Posición a la que queremos que vaya la cámara (siguiendo al jugador en X e Y, manteniendo Z)
        Vector3 targetPosition = new Vector3(objectToFollow.position.x + offset.x, objectToFollow.position.y + offset.y, offset.z);

        // Lógica del Temblor de Cámara (Shake)
        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0; // Mantenemos la Z intacta
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            shakeOffset = Vector3.zero;
        }

        // Mover la cámara suavemente hacia esa posición usando Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime) + shakeOffset;
    }

    /// <summary>
    /// Método público para activar el temblor de cámara cuando el jugador recibe castigo de velocidad.
    /// </summary>
    /// <param name="duration">Duración del temblor en segundos</param>
    /// <param name="magnitude">Fuerza o intensidad de la sacudida</param>
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}