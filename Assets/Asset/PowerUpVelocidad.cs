using UnityEngine;
using System.Collections;

public class PowerUpVelocidad : MonoBehaviour
{
    public float duration = 5f;
    public float multiplicadorVelocidad = 2f;
    
    void Start()
    {
        StartCoroutine(Rotar());
    }
    
    IEnumerator Rotar()
    {
        while (true)
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0);
            yield return null;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 💥 PARCHE DINÁMICO: Buscamos cualquier script que tenga la variable 'velocidad' en tu jugador
            // para saltarnos el error de si la clase empieza con mayúscula o minúscula.
            Component scriptMovimiento = other.GetComponent("movimiento") ?? other.GetComponent("ControlJugador");
            
            if (scriptMovimiento != null)
            {
                GameManager gm = FindFirstObjectByType<GameManager>();
                if (gm != null) gm.MostrarLetreroVelocidad(true);

                StartCoroutine(AumentarVelocidadDinamica(scriptMovimiento));
                OcultarObjeto();
            }
        }
    }

    void OcultarObjeto()
    {
        if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
        Destroy(gameObject, duration + 0.5f); 
    }
    
    IEnumerator AumentarVelocidadDinamica(Component script)
    {
        // Usamos Reflection básico de C# para leer la variable 'velocidad' sin importar el nombre de la clase
        var field = script.GetType().GetField("velocidad");
        if (field == null) yield break;

        float velocidadOriginal = (float)field.GetValue(script);
        field.SetValue(script, velocidadOriginal * multiplicadorVelocidad);
        
        yield return new WaitForSeconds(duration);
        
        field.SetValue(script, velocidadOriginal);

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.MostrarLetreroVelocidad(false);
    }
}