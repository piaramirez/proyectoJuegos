using UnityEngine;

public class BotonPresion : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip sonidoActivar;
    public float pesoRequerido = 2f;
    
    private AudioSource audioSource;
    private bool activado = false;
    private Renderer rend;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
    }
    
    void OnTriggerStay(Collider other)
    {
        if (activado) return;
        
        float pesoActual = 0f;
        
        // Buscar objetos con tag Recogible cerca
        Collider[] objetos = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider col in objetos)
        {
            if (col.CompareTag("Recogible"))
            {
                ObjetoRecogible obj = col.GetComponent<ObjetoRecogible>();
                if (obj != null)
                {
                    pesoActual += obj.peso;  // ← AHORA SÍ EXISTE (se llama "peso")
                    Debug.Log($"📦 Objeto detectado! Peso: {obj.peso}");
                }
            }
        }
        
        // Peso del jugador
        if (other.CompareTag("Player"))
        {
            pesoActual += 1f;
            Debug.Log($"👤 Jugador en botón. Peso total: {pesoActual}");
        }
        
        if (pesoActual >= pesoRequerido && !activado)
        {
            activado = true;
            rend.material.color = Color.green;
            if (sonidoActivar != null) audioSource.PlayOneShot(sonidoActivar);
            if (gameManager != null) gameManager.ActivarPuente();
            Debug.Log("✅ BOTÓN ACTIVADO! Peso: " + pesoActual);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}