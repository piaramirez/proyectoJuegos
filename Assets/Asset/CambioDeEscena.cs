using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreEscenaDestino = "Nivel2";
    public float tiempoEspera = 1f;
    
    [Header("Audios")]
    public AudioClip sonidoTransicion;
    
    private AudioSource audioSource;
    private bool transicionando = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (transicionando) return;
        
        if (other.CompareTag("Player"))
        {
            transicionando = true;
            
            if (sonidoTransicion != null)
                audioSource.PlayOneShot(sonidoTransicion, 1f);
            
            Debug.Log($"🚪 Cargando escena: {nombreEscenaDestino}");
            
            // Guardar progreso
            PlayerPrefs.SetInt("UltimoNivelCompletado", ObtenerNivelActual());
            PlayerPrefs.Save();
            
            Invoke(nameof(CargarSiguienteEscena), tiempoEspera);
        }
    }
    
    void CargarSiguienteEscena()
    {
        SceneManager.LoadScene(nombreEscenaDestino);
    }
    
    int ObtenerNivelActual()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        if (escenaActual.Contains("Nivel1")) return 1;
        if (escenaActual.Contains("Nivel2")) return 2;
        return 0;
    }
}