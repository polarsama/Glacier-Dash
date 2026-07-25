using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public float speed = 8f;
    public float dashForce = 15f;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

 
    void Update()
    {
        // Movimiento constante hacia la derecha en el eje X
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // Deteccion con el nuevo Input System de Unity 6
        // Revisa si presionales Espacio en el teclado o el Clic Izquierdo en el raton
        bool pressSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool pressLeftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (pressSpace || pressLeftClick)
        {
            ExecuteDash();
        }
    }

    void ExecuteDash()
    {
        // Aplica el impulso instantaneo
        rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
    }
}
