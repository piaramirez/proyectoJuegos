using UnityEngine;

public class ObjetoCuracion : MonoBehaviour
{
    public float cantidadCuracion = 30f;
    public AudioClip sonidoAlCurar;
    public GameObject efectoAlCurar;
    
    private AudioSource audioSource;
    private bool usado = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (usado) return;
        
        if (other.CompareTag("Player"))
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();
            
            if (vidaJugador != null)
            {
                if (vidaJugador.GetVidaActual() < vidaJugador.GetVidaMaxima())
                {
                    vidaJugador.Curar(cantidadCuracion);
                    usado = true;
                    
                    if (sonidoAlCurar != null)
                    {
                        audioSource.PlayOneShot(sonidoAlCurar);
                        Debug.Log("🔊 Sonido de curación - durará 2 segundos");
                    }
                    
                    if (efectoAlCurar != null)
                        Instantiate(efectoAlCurar, transform.position, Quaternion.identity);
                    
                    // Ocultar el objeto visualmente
                    MeshRenderer mr = GetComponent<MeshRenderer>();
                    if (mr != null) mr.enabled = false;
                    
                    GetComponent<Collider>().enabled = false;
                    
                    // ESPERAR 2 SEGUNDOS ANTES DE DESTRUIR
                    Destroy(gameObject, 2f);
                }
                else
                {
                    Debug.Log("Vida al máximo, no se puede curar");
                }
            }
        }
    }
}