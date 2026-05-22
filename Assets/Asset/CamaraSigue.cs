using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador;

    [Header("Configuración de Vista Isométrica")]
    // Estos valores determinan qué tan alejada y elevada está la cámara
    public Vector3 offset = new Vector3(0, 15f, -15f); // Aumentado para más distancia
    
    // Ángulo de inclinación hacia abajo (X) y rotación (Y)
    public Vector3 angle = new Vector3(45f, 0, 0); // 45 grados hacia abajo para perspectiva 3D

    [Header("Suavizado")]
    public float suavizado = 5f;
    
    void Start()
    {
        // Aplicar la rotación isométrica fija al inicio
        transform.rotation = Quaternion.Euler(angle);
    }

    void LateUpdate()
    {
        if (jugador == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) jugador = player.transform;
            return;
        }
        
        // Calculamos la posición deseada basándonos en el offset
        Vector3 posicionDeseada = jugador.position + offset;
        
        // Seguimiento suave de posición
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}