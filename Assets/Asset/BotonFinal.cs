using UnityEngine;

public class BotonFinal : MonoBehaviour
{
    public GameManager gameManager;  // ← Arrastra GameManager aquí
    public AudioClip sonidoActivar;
    
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
        
        // Buscar GameManager si no está asignado
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (activado) return;
        
        if (other.CompareTag("Player"))
        {
            activado = true;
            rend.material.color = Color.green;
            
            if (sonidoActivar != null)
                audioSource.PlayOneShot(sonidoActivar);
            
            if (gameManager != null)
                gameManager.ActivarPuente();
            else
                Debug.LogError("❌ GameManager NO asignado en BotonFinal!");
            
            Debug.Log("✅ Botón activado! Puente debería aparecer");
        }
    }
}