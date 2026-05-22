using UnityEngine;

public class BotonPeso : MonoBehaviour
{
    public GameManager gameManager;
    public float pesoRequerido = 2f;
    
    private bool activado = false;
    private Renderer rend;
    private float pesoActual = 0f;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
    }
    
    void OnTriggerStay(Collider other)
    {
        if (activado) return;
        
        pesoActual = 0f;
        
        // Detectar caja
        Collider[] objetos = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider col in objetos)
        {
            if (col.CompareTag("Recogible"))
            {
                ObjetoRecogible obj = col.GetComponent<ObjetoRecogible>();
                if (obj != null)
                {
                    pesoActual += obj.peso;
                    Debug.Log($"📦 Caja en botón! Peso: {obj.peso}");
                }
            }
        }
        
        // Detectar jugador
        if (other.CompareTag("Player"))
        {
            pesoActual += 1f;
        }
        
        if (pesoActual >= pesoRequerido && !activado)
        {
            activado = true;
            rend.material.color = Color.green;
            if (gameManager != null) gameManager.ActivarPuente();
            Debug.Log("✅ BOTÓN ACTIVADO!");
        }
    }
}