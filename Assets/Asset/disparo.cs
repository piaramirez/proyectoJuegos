using System.Collections;
using UnityEngine;

public class PlayerDisparo : MonoBehaviour
{
    public int balasEnCargador = 15;
    public int balasTotales = 100;
    public int capacidadCargador = 15;
    public float tiempoEntreDisparos = 0.2f;
    public GameObject prefabBala;
    public Transform puntoDisparo;
    
    public AudioClip sonidoDisparo;
    public AudioClip sonidoSinBalas;
    public AudioClip sonidoRecargar;
    
    private AudioSource audioSource; 
    private float siguienteTiempoDisparo;
    private bool estaRecargando = false;

    void Start() {
        balasEnCargador = capacidadCargador;
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; 
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
                audioSource.PlayOneShot(sonidoDisparo, 1f); 
            }
            
            if (prefabBala != null && puntoDisparo != null)
            {
                // 🔥 ALTURA CONGELADA: Nace siempre a 1.1 metros de altura del suelo real
                Vector3 posicionForzada = new Vector3(puntoDisparo.position.x, 1.1f, puntoDisparo.position.z);
                Instantiate(prefabBala, posicionForzada, puntoDisparo.rotation);
            }
        } else {
            if (sonidoSinBalas != null) {
                audioSource.PlayOneShot(sonidoSinBalas, 1f);
            }
        }

        UIManager ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.ActualizarMunicion();
    }

    IEnumerator Recargar() {
        estaRecargando = true;
        yield return new WaitForSeconds(1.5f);
        
        int balasNecesarias = capacidadCargador - balasEnCargador;
        int balasARecargar = Mathf.Min(balasNecesarias, balasTotales);
        balasEnCargador += balasARecargar;
        balasTotales -= balasARecargar;
        estaRecargando = false;
        
        UIManager ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.ActualizarMunicion();
    }
}