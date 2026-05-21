using UnityEngine;

public class BotonPeso : MonoBehaviour
{
    [Header("Configuración")]
    public float pesoRequerido = 1f;
    public GameObject objetoAActivar;   // Puente, Puerta, etc.
    public bool activarUnaSolaVez = true;
    
    [Header("Visuales")]
    public Material materialActivado;
    public Material materialDesactivado;
    public Color colorActivado = Color.green;
    public Color colorDesactivado = Color.red;
    
    [Header("Audios")]
    public AudioClip sonidoActivar;
    public AudioClip sonidoDesactivar;
    
    private Renderer rend;
    private AudioSource audioSource;
    private bool estaActivado = false;
    private bool yaActivo = false;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        ActualizarVisual(false);
    }
    
    void OnTriggerStay(Collider other)
    {
        if (activarUnaSolaVez && yaActivo) return;
        
        if (other.CompareTag("Player"))
        {
            float pesoJugador = ObtenerPesoJugador(other.gameObject);
            
            if (pesoJugador >= pesoRequerido && !estaActivado)
            {
                Activar();
            }
            else if (pesoJugador < pesoRequerido && estaActivado && !activarUnaSolaVez)
            {
                Desactivar();
            }
        }
    }
    
    float ObtenerPesoJugador(GameObject jugador)
    {
        // Buscar el sistema de peso
        WeightSystem peso = jugador.GetComponent<WeightSystem>();
        if (peso != null)
            return peso.GetCurrentWeight();
        
        // Si no hay sistema de peso, el jugador pesa 1
        return 1f;
    }
    
    void Activar()
    {
        estaActivado = true;
        yaActivo = true;
        
        // Activar el objeto (puente, puerta, etc.)
        if (objetoAActivar != null)
        {
            // Si es una puerta, la desactivamos (desaparece)
            if (objetoAActivar.CompareTag("Puerta"))
                objetoAActivar.SetActive(false);
            else
                objetoAActivar.SetActive(true);
        }
        
        // Sonido
        if (sonidoActivar != null)
            audioSource.PlayOneShot(sonidoActivar, 1f);
        
        // Visual
        ActualizarVisual(true);
        
        Debug.Log($"✅ Botón ACTIVADO! Peso: {pesoRequerido}");
    }
    
    void Desactivar()
    {
        estaActivado = false;
        
        if (objetoAActivar != null)
        {
            if (objetoAActivar.CompareTag("Puerta"))
                objetoAActivar.SetActive(true);
            else
                objetoAActivar.SetActive(false);
        }
        
        if (sonidoDesactivar != null)
            audioSource.PlayOneShot(sonidoDesactivar, 0.7f);
        
        ActualizarVisual(false);
        
        Debug.Log($"❌ Botón DESACTIVADO");
    }
    
    void ActualizarVisual(bool activado)
    {
        if (rend != null)
        {
            if (activado && materialActivado != null)
                rend.material = materialActivado;
            else if (!activado && materialDesactivado != null)
                rend.material = materialDesactivado;
            else if (activado)
                rend.material.color = colorActivado;
            else
                rend.material.color = colorDesactivado;
        }
    }
    
    // Para debug en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}