using UnityEngine;
using System.Collections;

public class WeightSystem : MonoBehaviour
{
    public float pesoBase = 1f;
    private float pesoActual;
    private float pesoModificado = 0f;
    
    void Start()
    {
        pesoActual = pesoBase;
    }
    
    public void AgregarPeso(float peso)  // ← MÉTODO AGREGADO
    {
        pesoModificado += peso;
        pesoActual = pesoBase + pesoModificado;
        Debug.Log($"⚖️ Peso actual: {pesoActual}");
    }
    
    public void QuitarPeso(float peso)
    {
        pesoModificado -= peso;
        if (pesoModificado < 0) pesoModificado = 0;
        pesoActual = pesoBase + pesoModificado;
        Debug.Log($"⚖️ Peso actual: {pesoActual}");
    }
    
    public float GetCurrentWeight()
    {
        return pesoActual;
    }
    
    public void ResetPeso()
    {
        pesoModificado = 0;
        pesoActual = pesoBase;
    }
}