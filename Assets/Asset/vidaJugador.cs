using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaMaxima = 100f;
    [SerializeField] private float vidaActual;
    public bool estaMuerto = false;

    private Dictionary<Renderer, Color> coloresOriginales = new Dictionary<Renderer, Color>();
    private Renderer[] misRenderers;
    
    void Start()
    {
        vidaActual = vidaMaxima;
        
        misRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in misRenderers)
        {
            if (r != null && r.material.HasProperty("_Color"))
            {
                coloresOriginales[r] = r.material.color;
            }
        }

        ActualizarUI();
    }
    
    public void RecibirDano(float cantidad)
    {
        Debug.Log($"\U0001FA78 [VidaJugador] RecibirDano invocado. Cantidad entrante: {cantidad}. Vida actual previa: {vidaActual}");
        
        if (estaMuerto) return;
        
        vidaActual -= cantidad;
        
        StopCoroutine(AnimacionDaño());
        StartCoroutine(AnimacionDaño());
        
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
        }
        
        ActualizarUI();
        Debug.Log($"❤️ Vida Jugador Actualizada: {vidaActual}/{vidaMaxima}");
    }
    
    public void Curar(float cantidad)
    {
        if (estaMuerto) return;
        
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
        
        ActualizarUI();
        Debug.Log($"💚 Vida Jugador: {vidaActual}/{vidaMaxima}");
    }
    
    void ActualizarUI()
    {
        UIManager ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.ActualizarVida();
    }
    
    IEnumerator AnimacionDaño()
    {
        foreach (Renderer r in misRenderers)
        {
            if (r != null) r.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.2f);
        
        foreach (Renderer r in misRenderers)
        {
            if (r != null && coloresOriginales.ContainsKey(r))
            {
                r.material.color = coloresOriginales[r];
            }
        }
    }
    
    void Morir()
    {
        estaMuerto = true;
        Debug.Log("💀 El personaje principal ha muerto.");

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.TerminarPorDerrota();
    }
    
    public float GetVidaActual() { return vidaActual; }
    public float GetVidaMaxima() { return vidaMaxima; }
}