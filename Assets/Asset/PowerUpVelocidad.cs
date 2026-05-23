using System.Collections;
using UnityEngine;

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
            Component scriptMovimiento = other.GetComponent("movimiento") ?? other.GetComponent("ControlJugador");
            
            if (scriptMovimiento != null)
            {
                // 🔥 CORRECCIÓN API: Actualizado para Builds WebGL
                GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));
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
        var field = script.GetType().GetField("velocidad");
        if (field == null) yield break;

        float velocidadOriginal = (float)field.GetValue(script);
        field.SetValue(script, velocidadOriginal * multiplicadorVelocidad);
        
        yield return new WaitForSeconds(duration);
        
        field.SetValue(script, velocidadOriginal);

        // 🔥 CORRECCIÓN API: Actualizado para Builds WebGL
        GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));
        if (gm != null) gm.MostrarLetreroVelocidad(false);
    }
}