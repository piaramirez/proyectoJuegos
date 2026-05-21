using UnityEngine;

public class PuertaFinal : MonoBehaviour
{
    public string siguienteEscena = "Nivel2";
    public AudioClip sonidoAbrir;
    
    private AudioSource audioSource;
    private bool abierta = false;
    private UIManager uiManager;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        uiManager = FindObjectOfType<UIManager>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !abierta)
        {
            if (uiManager != null && uiManager.TieneLlave())
            {
                AbrirPuerta();
            }
            else
            {
                Debug.Log("❌ Necesitas la llave para abrir esta puerta");
            }
        }
    }
    
    void AbrirPuerta()
    {
        abierta = true;
        if (sonidoAbrir != null)
            audioSource.PlayOneShot(sonidoAbrir);
        
        if (uiManager != null)
            uiManager.AbrirPuerta();
        
        // Animación de abrir
        transform.Translate(Vector3.up * 2f);
        
        Invoke("CargarSiguienteNivel", 2f);
    }
    
    void CargarSiguienteNivel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(siguienteEscena);
    }
}