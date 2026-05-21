using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset = new Vector3(0, 5, -8);
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (jugador == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) jugador = player.transform;
            return;
        }

        Vector3 posicionDeseada = jugador.position + offset;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}