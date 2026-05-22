using UnityEngine;

public class WeightPowerUp : MonoBehaviour
{
    [SerializeField] private float duracion = 5f; // Se usa ahora
    
    void Start()
    {
        Debug.Log($"Power-up creado con duración: {duracion}");
    }
}