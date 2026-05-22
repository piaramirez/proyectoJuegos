using UnityEngine;

public class RotarJugador : MonoBehaviour
{
    public float velocidadRotacion = 10f;

    void Update()
    {
        // Obtiene el input de las flechas o WASD
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccionMovimiento = new Vector3(horizontal, 0f, vertical).normalized;

        // Si se está presionando alguna tecla de movimiento, rota hacia allá
        if (direccionMovimiento.magnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
        }
    }
}