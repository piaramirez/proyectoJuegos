using UnityEngine;
using UnityEngine.Events;

public class WeightButton : MonoBehaviour
{
    [Header("Configuración de Peso")]
    [SerializeField] private float pesoRequerido = 1f;  // Peso mínimo para activar
    
    [Header("Objetos que controla")]
    [SerializeField] private GameObject[] objetosAActivar;  // Puertas, plataformas, etc.
    [SerializeField] private GameObject[] objetosADesactivar;
    
    [Header("Eventos")]
    public UnityEvent onActivado;
    public UnityEvent onDesactivado;
    
    [Header("Visuales")]
    [SerializeField] private Material materialActivado;
    [SerializeField] private Material materialDesactivado;
    private Renderer buttonRenderer;
    private bool estaActivado = false;
    private float pesoActualEnBotón = 0f;
    
    [Header("Audios")]
    [SerializeField] private AudioClip sonidoActivacion;
    [SerializeField] private AudioClip sonidoDesactivacion;
    private AudioSource audioSource;
    
    void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        ActualizarVisual();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ControlJugador jugador = other.GetComponent<ControlJugador>();
            if (jugador != null)
            {
                // Obtener el peso actual del jugador (lo agregaremos después)
                float pesoJugador = GetPesoJugador(jugador);
                pesoActualEnBotón += pesoJugador;
                VerificarActivacion();
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ControlJugador jugador = other.GetComponent<ControlJugador>();
            if (jugador != null)
            {
                float pesoJugador = GetPesoJugador(jugador);
                pesoActualEnBotón -= pesoJugador;
                VerificarActivacion();
            }
        }
    }
    
    float GetPesoJugador(ControlJugador jugador)
    {
        // Buscar el componente WeightSystem en el jugador
        WeightSystem weightSys = jugador.GetComponent<WeightSystem>();
        if (weightSys != null)
            return weightSys.GetCurrentWeight();
        return 1f; // Peso por defecto
    }
    
    void VerificarActivacion()
    {
        bool deberiaActivar = pesoActualEnBotón >= pesoRequerido;
        
        if (deberiaActivar && !estaActivado)
        {
            Activar();
        }
        else if (!deberiaActivar && estaActivado)
        {
            Desactivar();
        }
    }
    
    void Activar()
    {
        estaActivado = true;
        
        // Activar objetos
        foreach (GameObject obj in objetosAActivar)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        
        // Desactivar objetos
        foreach (GameObject obj in objetosADesactivar)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        
        // Evento
        onActivado?.Invoke();
        
        // Sonido
        if (sonidoActivacion != null)
            audioSource.PlayOneShot(sonidoActivacion, 0.8f);
        
        // Visual
        ActualizarVisual();
        
        Debug.Log($"✅ Botón ACTIVADO (Peso: {pesoActualEnBotón}/{pesoRequerido})");
    }
    
    void Desactivar()
    {
        estaActivado = false;
        
        // Revertir objetos
        foreach (GameObject obj in objetosAActivar)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        
        foreach (GameObject obj in objetosADesactivar)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        
        onDesactivado?.Invoke();
        
        if (sonidoDesactivacion != null)
            audioSource.PlayOneShot(sonidoDesactivacion, 0.5f);
        
        ActualizarVisual();
        
        Debug.Log($"❌ Botón DESACTIVADO (Peso: {pesoActualEnBotón}/{pesoRequerido})");
    }
    
    void ActualizarVisual()
    {
        if (buttonRenderer != null)
        {
            if (estaActivado && materialActivado != null)
                buttonRenderer.material = materialActivado;
            else if (!estaActivado && materialDesactivado != null)
                buttonRenderer.material = materialDesactivado;
        }
    }
    
    // Mostrar el peso requerido en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        
        // Texto en el editor (opcional)
        UnityEditor.Handles.Label(transform.position + Vector3.up, $"Peso requerido: {pesoRequerido}");
    }
}