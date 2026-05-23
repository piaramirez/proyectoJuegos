using UnityEngine;

public class BotonPiso : MonoBehaviour
{
    private bool activado = false;
    private Renderer rendererBotón;

    void Start()
    {
        rendererBotón = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Se activa si lo pisa el jugador o si dejas caer la Caja
        if (other.CompareTag("Player") || other.CompareTag("Caja") || other.name.Contains("Caja") || other.name.Contains("Hero"))
        {
            ActivarBoton();
        }
    }

    void ActivarBoton()
    {
        if (activado) return;
        activado = true;

        if (rendererBotón != null) rendererBotón.material.color = Color.green; // Cambia a verde el botón

        // 🍏 EL PARCHE MAESTRO: Llama a ActivarPuente que es el método real de tu GameManager de 130 líneas
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.ActivarPuente();
            Debug.Log("🍏 ¡BOTÓN ACTIVADO! Mandando señal al GameManager para el siguiente paso del puzzle.");
        }
        else
        {
            Debug.LogError("❌ No se encontró el GameManager en la escena.");
        }
    }
}