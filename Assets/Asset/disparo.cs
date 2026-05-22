using System.Collections;
using UnityEngine;

public class PlayerDisparo : MonoBehaviour
{
    public int balasEnCargador = 15;
    public int balasTotales = 60;
    public int capacidadCargador = 15;
    public float tiempoEntreDisparos = 0.2f;
    public GameObject prefabBala;
    public Transform puntoDisparo;
    
    public AudioClip sonidoDisparo;
    public AudioClip sonidoSinBalas;
    public AudioClip sonidoRecargar;
    
    private AudioSource audioSource; // Usaremos un AudioSource fijo
    private float siguienteTiempoDisparo;
    private bool estaRecargando = false;

    void Start() {
        balasEnCargador = capacidadCargador;
        // Configuramos el AudioSource para que sea 2D y potente
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 0 = 2D TOTAL (Suena en toda la pantalla)
        audioSource.playOnAwake = false;
    }

    void Update() {
        if (Input.GetButtonDown("Fire1") && Time.time >= siguienteTiempoDisparo && !estaRecargando)
            Disparar();
        
        if (Input.GetKeyDown(KeyCode.R) && !estaRecargando && balasEnCargador < capacidadCargador)
            StartCoroutine(Recargar());
    }

void Disparar() {
        if (balasEnCargador > 0) {
            balasEnCargador--;
            siguienteTiempoDisparo = Time.time + tiempoEntreDisparos;
            
            if (sonidoDisparo != null) {
                // Usamos PlayOneShot para que el disparo suene AL MISMO TIEMPO que el salto o caminar
                audioSource.PlayOneShot(sonidoDisparo, 1f); 
            }
            
            Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
            Debug.Log($"Balas: {balasEnCargador}/{capacidadCargador}");
        } else {
            if (sonidoSinBalas != null) {
                audioSource.PlayOneShot(sonidoSinBalas, 1f);
            }
            Debug.Log("Sin balas");
        }
        // Al final de Disparar() y Recargar(), agrega:
UIManager ui = FindFirstObjectByType<UIManager>();
if (ui != null) ui.ActualizarMunicion();
    }

    IEnumerator Recargar() {
        estaRecargando = true;
        Debug.Log("Recargando");
        
        if (sonidoRecargar) audioSource.PlayOneShot(sonidoRecargar, 1f);
        
        yield return new WaitForSeconds(1.5f);
        
        int balasNecesarias = capacidadCargador - balasEnCargador;
        int balasARecargar = Mathf.Min(balasNecesarias, balasTotales);
        balasEnCargador += balasARecargar;
        balasTotales -= balasARecargar;
        estaRecargando = false;
        // Al final de Disparar() y Recargar(), agrega:
UIManager ui = FindFirstObjectByType<UIManager>();
if (ui != null) ui.ActualizarMunicion();
    }
    
}