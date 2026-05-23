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
        if (other.CompareTag("Player") || other.CompareTag("Caja") || other.name.Contains("Caja") || other.name.Contains("Hero"))
        {
            ActivarBoton(other.gameObject);
        }
    }

    void ActivarBoton(GameObject objetoQueToco)
    {
        if (activado) return;
        activado = true;

        if (rendererBotón != null) rendererBotón.material.color = Color.green; 

        GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));
        if (gm != null)
        {
            gm.ActivarPuente();
            Debug.Log("🍏 ¡BOTÓN ACTIVADO! Puente y lava procesados correctamente.");
        }
        else
        {
            Debug.LogError("❌ No se encontró el GameManager en la escena.");
        }

        // 🔥 DESAPARICIÓN DE LA CAJA
        if (objetoQueToco.CompareTag("Caja") || objetoQueToco.name.Contains("Caja"))
        {
            Destroy(objetoQueToco);
        }

        // 🔥 DESAPARICIÓN DEL BOTÓN (Con leve retraso para evitar interrupciones de hilos)
        Destroy(gameObject, 0.05f); 
    }
}