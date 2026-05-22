using UnityEngine;
using UnityEngine.AI;

public class EnemigoPerseguidor : MonoBehaviour
{
    public System.Action OnMuerte;

    [Header("Estadísticas")]
    public float vidaEnemigo = 50f;
    public float dañoAlJugador = 10f;
    public float tiempoEntreAtaques = 1.5f; // Coodown de 1.5 segundos

    [Header("Sonidos")]
    public AudioClip sonidoPasos;
    public AudioClip sonidoRecibirDaño;
    public AudioClip sonidoMorir;
    public AudioClip sonidoAtacar;

    private Transform jugador;
    private NavMeshAgent agente;
    private AudioSource audioSource;
    private float siguienteAtaque; // El reloj que frena el daño repetido
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

    // --- SISTEMA DE DAÑO CONTROLADO POR COOLDOWN ---

    void OnCollisionEnter(Collision col)
    {
        IntentarAtacar(col.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        IntentarAtacar(other.gameObject);
    }

    void OnCollisionStay(Collision col)
    {
        IntentarAtacar(col.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        IntentarAtacar(other.gameObject);
    }

    void IntentarAtacar(GameObject victima)
    {
        if (estaMuerto) return;

        // REGLA DE ORO: Si el tiempo actual no ha superado el cooldown, NO hace daño
        if (Time.time < siguienteAtaque) return;

        if (victima.CompareTag("Player"))
        {
            VidaJugador vida = victima.GetComponent<VidaJugador>();
            if (vida != null)
            {
                // Aplica el daño una sola vez
                vida.RecibirDano(dañoAlJugador);
                
                // BLOQUEO: Setea el tiempo para el próximo ataque (Tiempo actual + 1.5s)
                siguienteAtaque = Time.time + tiempoEntreAtaques;

                if (sonidoAtacar != null)
                    audioSource.PlayOneShot(sonidoAtacar, 0.7f);

                Debug.Log($"<color=red>⚔️ ENEMIGO: Ataque regulado. Te quité {dañoAlJugador}. Próximo ataque en {tiempoEntreAtaques}s</color>");
            }
        }
    }

    // --- FIN DEL SISTEMA DE DAÑO ---

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
        
        if(GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.EnemigoMuerto();

        UIManager ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.MatarEsqueleto();

        Destroy(gameObject, 2f);
    }
}