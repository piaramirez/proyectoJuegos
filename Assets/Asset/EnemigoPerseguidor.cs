using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic; 
using System;

public class EnemigoPerseguidor : MonoBehaviour
{
    [Header("Configuración IA")]
    public float daño = 10f;
    public float velocidad = 3.5f;
    public float distanciaAtaque = 2.5f; 
    public float tiempoEntreAtaques = 1.2f;

    [Header("Configuración de Vida")]
    public float vidaEnemigo = 30f; // 🔥 CADA BALA QUITA 10, AL TERCER GOLPE MUERE

    [Header("Configuración de Impacto")]
    public float fuerzaRetroceso = 6f; 

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Transform jugador;
    private float cronometroAtaque;
    private bool puedeAtacar = true;
    private bool estaMuriendo = false;
    private bool recibiendoKnockback = false;

    private Dictionary<Renderer, Color> coloresOriginales = new Dictionary<Renderer, Color>();
    private Renderer[] misRenderers;

    public event Action OnMuerte;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        
        if (agent != null) agent.speed = velocidad;
        if (rb != null) rb.isKinematic = true; 

        misRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in misRenderers)
        {
            if (r != null && r.material.HasProperty("_Color"))
                coloresOriginales[r] = r.material.color;
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) jugador = playerObj.transform;
    }

    void Update()
    {
        if (jugador == null || agent == null || !puedeAtacar || estaMuriendo || recibiendoKnockback) return;

        agent.SetDestination(jugador.position);
        cronometroAtaque += Time.deltaTime;

        float distanciaReal = Vector3.Distance(transform.position, jugador.position);

        if (distanciaReal <= distanciaAtaque && cronometroAtaque >= tiempoEntreAtaques)
        {
            AtacarJugador();
        }
    }

    void AtacarJugador()
    {
        cronometroAtaque = 0f;
        VidaJugador vida = FindFirstObjectByType<VidaJugador>(); 

        if (vida != null)
        {
            vida.RecibirDano(daño); 
        }
    }

    // 🔥 EL MÉTODO CORREGIDO: Ahora sí resta vida y mata al enemigo
    public void RecibirImpactoBala(Vector3 direccionBala)
    {
        if (estaMuriendo) return;

        // 1. Restamos vida (10 puntos por bala)
        vidaEnemigo -= 10f;
        Debug.Log($"🎯 [{gameObject.name}] ¡Impacto recibido! Vida restante del esqueleto: {vidaEnemigo}");

        // 2. Si se quedó sin vida, ejecuta la animación de muerte y se destruye
        if (vidaEnemigo <= 0)
        {
            StartCoroutine(EfectoMuerte());
            return;
        }

        // 3. Si aún le queda vida, solo retrocede y se pone rojo
        StartCoroutine(EfectoDañoYRetroceso(direccionBala));
    }

    IEnumerator EfectoDañoYRetroceso(Vector3 direccionBala)
    {
        recibiendoKnockback = true;

        if (agent != null) agent.enabled = false;
        if (rb != null) rb.isKinematic = false; 

        foreach (Renderer r in misRenderers)
        {
            if (r != null) r.material.color = Color.red;
        }

        Vector3 direccionEmpuje = new Vector3(direccionBala.x, 0f, direccionBala.z).normalized;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.AddForce(direccionEmpuje * fuerzaRetroceso, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.18f);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        foreach (Renderer r in misRenderers)
        {
            if (r != null && coloresOriginales.ContainsKey(r))
                r.material.color = coloresOriginales[r];
        }

        if (agent != null) agent.enabled = true;
        
        recibiendoKnockback = false;
    }

    // 🔥 CORUTINA DE MUERTE: Avisa al GameManager y limpia la escena
    IEnumerator EfectoMuerte()
    {
        estaMuriendo = true;
        puedeAtacar = false;
        if (agent != null) agent.enabled = false;
        
        Debug.Log($"💀 [{gameObject.name}] ¡El esqueleto ha sido destruido por completo!");

        // Lo pintamos de rojo un último instante antes de borrarlo
        foreach (Renderer r in misRenderers)
        {
            if (r != null) r.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.15f);

        // Le avisa al GameManager para que sume al contador de la UI (0/3)
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.EnemigoMuerto();

        OnMuerte?.Invoke();
        Destroy(gameObject);
    }
}