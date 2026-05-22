using UnityEngine;
using System.Collections;

public class VidaJugador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;
    public bool estaMuerto = false;
    
    void Start()
    {
        vidaActual = vidaMaxima;
    }
    
    public void RecibirDano(float cantidad)
    {
        if (estaMuerto) return;
        
        vidaActual -= cantidad;
        StartCoroutine(AnimacionDaño());
        
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
        }
        
        ActualizarUI();
        Debug.Log($"❤️ Vida: {vidaActual}/{vidaMaxima}");
    }
    
    public void Curar(float cantidad)
    {
        if (estaMuerto) return;
        
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
        
        ActualizarUI();
        Debug.Log($"💚 Vida: {vidaActual}/{vidaMaxima}");
    }
    
    void ActualizarUI()
    {
        UIManager ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.ActualizarVida();
    }
    
    IEnumerator AnimacionDaño()
    {
        Renderer[] renders = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renders)
        {
            r.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.2f);
        
        foreach (Renderer r in renders)
        {
            r.material.color = Color.white;
        }
    }
    
    void Morir()
    {
        estaMuerto = true;
        Debug.Log("💀 Jugador murió");
    }
    
    public float GetVidaActual() { return vidaActual; }
    public float GetVidaMaxima() { return vidaMaxima; }
}