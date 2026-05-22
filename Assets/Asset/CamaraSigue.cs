using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    public Transform jugador;

    [Header("Configuración de Vista Aérea 3D")]
    // Distancia respecto al jugador (Y: altura, Z: hacia atrás)
    public Vector3 offset = new Vector3(0f, 12f, -12f);
    
    // Inclinación fija de 45 grados hacia abajo mirando al escenario
    public float inclinacionX = 45f;

    [Header("Suavizado")]
    public float velocidadSeguimiento = 5f;

    void LateUpdate()
    {
        // Si no tienes asignado al jugador, lo busca por su Tag automáticamente
        if (jugador == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) jugador = player.transform;
            return;
        }

        // 1. Forzar la posición calculando el offset exacto desde el jugador
        Vector3 posicionDeseada = jugador.position + offset;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, velocidadSeguimiento * Time.deltaTime);

        // 2. Forzar la rotación para que NUNCA se voltee de cabeza ni mire de frente
        transform.rotation = Quaternion.Euler(inclinacionX, 0f, 0f);
    }
}