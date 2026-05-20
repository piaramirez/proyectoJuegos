using UnityEngine;
using System.Collections;

public class WeightSystem : MonoBehaviour
{
    [Header("Peso del Jugador")]
    [SerializeField] private float pesoBase = 1f;
    [SerializeField] private float pesoActual;
    
    [Header("Power-up")]
    [SerializeField] private float pesoModificado = 2f;
    [SerializeField] private float duracionPowerUp = 5f;
    
    [Header("Visuales")]
    [SerializeField] private Material materialNormal;
    [SerializeField] private Material materialPesado;
    private Renderer playerRenderer;
    
    private bool estaModificado = false;
    private Coroutine rutinaPeso;
    
    public float PesoActual => pesoActual;
    public float PesoBase => pesoBase;
    public bool EstaPesado => pesoActual > pesoBase;
    
    void Start()
    {
        pesoActual = pesoBase;
        playerRenderer = GetComponentInChildren<Renderer>();
        ActualizarVisual();
    }
    
    public void ActivarPowerUp()
    {
        if (rutinaPeso != null)
            StopCoroutine(rutinaPeso);
        
        rutinaPeso = StartCoroutine(CambiarPesoTemporal());
    }
    
    IEnumerator CambiarPesoTemporal()
    {
        estaModificado = true;
        pesoActual = pesoModificado;
        ActualizarVisual();
        
        Debug.Log($"⚡ Power-up ACTIVADO! Peso: {pesoActual}");
        
        yield return new WaitForSeconds(duracionPowerUp);
        
        pesoActual = pesoBase;
        estaModificado = false;
        ActualizarVisual();
        
        Debug.Log($"⚡ Power-up DESACTIVADO. Peso normal: {pesoActual}");
    }
    
    void ActualizarVisual()
    {
        if (playerRenderer != null && materialPesado != null)
        {
            if (estaModificado)
                playerRenderer.material = materialPesado;
            else if (materialNormal != null)
                playerRenderer.material = materialNormal;
        }
    }
    
    // Método para que otros scripts consulten el peso
    public float GetCurrentWeight()
    {
        return pesoActual;
    }
}