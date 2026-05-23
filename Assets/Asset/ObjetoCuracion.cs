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
            if (vidaJugador == null) vidaJugador = other.GetComponentInChildren<VidaJugador>();
            
            if (vidaJugador != null)
            {
                if (vidaJugador.GetVidaActual() < vidaJugador.GetVidaMaxima())
                {
                    vidaJugador.Curar(cantidadCuracion);
                    usado = true;
                    
                    // 🔥 LLAMADA AL LETRERO VERDE DEL GAMEMANAGER
                    GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));
                    if (gm != null) gm.MostrarLetreroCuracion();
                    
                    if (sonidoAlCurar != null)
                    {
                        audioSource.PlayOneShot(sonidoAlCurar);
                    }
                    
                    if (efectoAlCurar != null)
                        Instantiate(efectoAlCurar, transform.position, Quaternion.identity);
                    
                    MeshRenderer mr = GetComponent<MeshRenderer>();
                    if (mr != null) mr.enabled = false;
                    
                    GetComponent<Collider>().enabled = false;
                    Destroy(gameObject, 1.8f);
                }
            }
        }
    }
}