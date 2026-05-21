using UnityEngine;
using UnityEngine.AI;

public class EnemigoPerseguidor : MonoBehaviour
{
    private Transform jugador;
    private NavMeshAgent agente;
    
    [Header("Estadísticas")]
    public float vidaEnemigo = 50f;
    public float dañoAlJugador = 15f;
    public float tiempoEntreAtaques = 1.5f;
    private float siguienteAtaque;

    [Header("Sonidos")]
    public AudioClip sonidoPasos;
    public AudioClip sonidoRecibirDaño;
    public AudioClip sonidoMorir;
    private AudioSource audioSource;
    
    public float intervaloPasos = 0.5f;
    private float temporizadorPasos;
    private bool estaMuerto = false;

    void Start() {
        agente = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) jugador = playerObj.transform;
    }

    void Update() {
        if (estaMuerto || jugador == null) return;
        
        agente.SetDestination(jugador.position);

        // Lógica de pasos (Volumen bajo)
        if (agente.velocity.magnitude > 0.1f) {
            temporizadorPasos += Time.deltaTime;
            if (temporizadorPasos >= intervaloPasos) {
                audioSource.PlayOneShot(sonidoPasos, 0.15f);
                temporizadorPasos = 0f;
            }
        }
    }

    // --- AQUÍ ESTÁ LO QUE FALTABA: EL ATAQUE ---
    void OnCollisionStay(Collision col) {
        if (estaMuerto) return;

        // Si toca al jugador y ya pasó el tiempo de enfriamiento
        if (col.gameObject.CompareTag("Player") && Time.time >= siguienteAtaque) {
            VidaJugador scriptVida = col.gameObject.GetComponent<VidaJugador>();
            if (scriptVida != null) {
                scriptVida.RecibirDano(dañoAlJugador);
                siguienteAtaque = Time.time + tiempoEntreAtaques;
                Debug.Log("¡El enemigo te mordió!");
            }
        }
    }

public void RecibirDaño(float cantidad) {
    if (estaMuerto) return;
    vidaEnemigo -= cantidad;

    if (sonidoRecibirDaño != null) {
        // Lo disparamos dos veces seguidas para que suene con más potencia
        audioSource.PlayOneShot(sonidoRecibirDaño, 1f); 
        audioSource.PlayOneShot(sonidoRecibirDaño, 1f); 
    }

    if (vidaEnemigo <= 0) Morir();
}

    void Morir() {
        estaMuerto = true;
        if (sonidoMorir) audioSource.PlayOneShot(sonidoMorir, 1f);
        agente.isStopped = true;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
        UIManager ui = FindObjectOfType<UIManager>();
    if (ui != null)
    {
        ui.MatarEsqueleto();
    }
    }
    
}