using UnityEngine;
using UnityEngine.AI;

public class EnemigoPerseguidor : MonoBehaviour
{
    public System.Action OnMuerte;  // Evento para el spawner
    
    [Header("Estadísticas")]
    public float vidaEnemigo = 50f;
    public float dañoAlJugador = 15f;
    public float tiempoEntreAtaques = 1.5f;
    
    [Header("Sonidos")]
    public AudioClip sonidoPasos;
    public AudioClip sonidoRecibirDaño;
    public AudioClip sonidoMorir;
    public AudioClip sonidoAtacar;
    
    private Transform jugador;
    private NavMeshAgent agente;
    private AudioSource audioSource;
    private float siguienteAtaque;
    private float temporizadorPasos;
    private bool estaMuerto = false;
    
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) jugador = player.transform;
    }
    
    void Update()
    {
        if (estaMuerto || jugador == null) return;
        
        agente.SetDestination(jugador.position);
        
        // Sonido de pasos
        if (agente.velocity.magnitude > 0.1f)
        {
            temporizadorPasos += Time.deltaTime;
            if (temporizadorPasos >= 0.5f && sonidoPasos != null)
            {
                audioSource.PlayOneShot(sonidoPasos, 0.3f);
                temporizadorPasos = 0f;
            }
        }
    }
    
    void OnCollisionStay(Collision col)
    {
        if (estaMuerto) return;
        
        if (col.gameObject.CompareTag("Player") && Time.time >= siguienteAtaque)
        {
            VidaJugador vida = col.gameObject.GetComponent<VidaJugador>();
            if (vida != null)
            {
                vida.RecibirDano(dañoAlJugador);
                siguienteAtaque = Time.time + tiempoEntreAtaques;
                
                if (sonidoAtacar != null)
                    audioSource.PlayOneShot(sonidoAtacar, 0.7f);
                
                Debug.Log($"⚔️ Esqueleto atacó! Daño: {dañoAlJugador}");
            }
        }
    }
    
    public void RecibirDaño(float cantidad)
    {
        if (estaMuerto) return;
        
        vidaEnemigo -= cantidad;
        
        if (sonidoRecibirDaño != null)
            audioSource.PlayOneShot(sonidoRecibirDaño, 0.8f);
        
        if (vidaEnemigo <= 0) Morir();
    }
    
    void Morir()
{
    estaMuerto = true;
    if (sonidoMorir) audioSource.PlayOneShot(sonidoMorir, 1f);
    agente.isStopped = true;
    GetComponent<Collider>().enabled = false;
    
    // NOTIFICAR AL GAME MANAGER
    GameManager gm = FindFirstObjectByType<GameManager>();
if (gm != null) gm.EnemigoMuerto();
    
    // NOTIFICAR AL UI
    UIManager ui = FindFirstObjectByType<UIManager>();
    if (ui != null) ui.MatarEsqueleto();
    
    Destroy(gameObject, 2f);
    
}
}