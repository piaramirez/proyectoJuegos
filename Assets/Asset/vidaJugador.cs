using UnityEngine;
using System.Collections;

public class VidaJugador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;
    public bool estaMuerto = false;
    
    public AudioClip sonidoDaño;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoCuracion;
    
    public GameObject efectoMuerte;
    private AudioSource audioSourceEfectos;
    
    void Start()
    {
        vidaActual = vidaMaxima;
        audioSourceEfectos = gameObject.AddComponent<AudioSource>();
        audioSourceEfectos.volume = 1f;
        audioSourceEfectos.playOnAwake = false;
    }
    
    public void RecibirDano(float cantidad)
    {
        if (estaMuerto) return;
        
        vidaActual -= cantidad;
        
        if (sonidoDaño != null)
            audioSourceEfectos.PlayOneShot(sonidoDaño, 1f);
        
        // Único log de vida al recibir daño
        Debug.Log($"Vida: {vidaActual}/{vidaMaxima}");
        
        if (vidaActual <= 0) Morir();
    }
    
    public void Curar(float cantidad)
    {
        if (estaMuerto) return;
        
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
        
        if (sonidoCuracion != null)
        {
            audioSourceEfectos.Stop(); 
            audioSourceEfectos.clip = sonidoCuracion;
            audioSourceEfectos.Play();
            
            // Log de inicio
            Debug.Log("🔊 Inicio de sonido de curación");
            
            StartCoroutine(DetenerSonidoCuracion(2f));
        }
    }

    IEnumerator DetenerSonidoCuracion(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        
        if (audioSourceEfectos.clip == sonidoCuracion)
        {
            audioSourceEfectos.Stop();
            // Log de terminación
            Debug.Log("🔇 Terminación de sonido de curación");
        }
    }
    
    void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
        
        if (sonidoMuerte != null)
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position, 1f);
        
        if (efectoMuerte != null)
            Instantiate(efectoMuerte, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
    
    public float GetVidaActual() { return vidaActual; }
    public float GetVidaMaxima() { return vidaMaxima; }
}