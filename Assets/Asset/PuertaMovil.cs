using UnityEngine;

public class PuenteMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;
    public bool activado = false;
    
    private Vector3 destino;
    
    void Start()
    {
        destino = puntoA.position;
    }
    
    void Update()
    {
        if (activado)
        {
            destino = puntoB.position;
        }
        else
        {
            destino = puntoA.position;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }
    
    public void Activar()
    {
        activado = true;
    }
    
    public void Desactivar()
    {
        activado = false;
    }
}