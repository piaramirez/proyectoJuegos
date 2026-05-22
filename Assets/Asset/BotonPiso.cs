using UnityEngine;

public class BotonPiso : MonoBehaviour
{
    private GameManager gameManager;
    private bool yaSePiso = false;

    void Start()
    {
        // Busca automáticamente al GameManager en la escena al iniciar
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Evita que se active más de una vez si lo pisas de nuevo
        if (yaSePiso) return;

        // Detecta si lo pisa el jugador (Player) o la caja (Caja)
        if (other.CompareTag("Player") || other.CompareTag("Caja"))
        {
            if (gameManager != null)
            {
                yaSePiso = true;
                gameManager.ActivarPuente();
                
                // Opcional: bajar un poco el botón visualmente para dar efecto de que se presionó
                transform.position -= new Vector3(0, 0.05f, 0); 
            }
        }
    }
}