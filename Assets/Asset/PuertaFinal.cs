using UnityEngine;

public class PuertaFinal : MonoBehaviour
{
    public GameManager gameManager;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.CompletarNivel();
            Debug.Log("🏆 Puerta tocada! Nivel completado");
        }
    }
}