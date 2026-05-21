using UnityEngine;

public class BotonPuente : MonoBehaviour
{
    public PuenteMovil puente;
    public Material materialActivado;
    public Material materialDesactivado;
    public AudioClip sonidoActivar;
    
    private Renderer rend;
    private AudioSource audioSource;
    private bool activo = false;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        ActualizarVisual();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activo = !activo;
            
            if (puente != null)
            {
                if (activo)
                    puente.Activar();
                else
                    puente.Desactivar();
            }
            
            if (sonidoActivar != null)
                audioSource.PlayOneShot(sonidoActivar);
            
            ActualizarVisual();
        }
    }
    
    void ActualizarVisual()
    {
        if (rend != null)
        {
            if (activo && materialActivado != null)
                rend.material = materialActivado;
            else if (!activo && materialDesactivado != null)
                rend.material = materialDesactivado;
        }
    }
}