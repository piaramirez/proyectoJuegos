using UnityEngine;
using System.Collections;

public class PowerUpVelocidad : MonoBehaviour
{
    public float duracion = 5f;
    public float multiplicadorVelocidad = 2f;
    
    void Start()
    {
        StartCoroutine(Rotar());
        Debug.Log("⚡ Power-up de velocidad creado");
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
            ControlJugador jugador = other.GetComponent<ControlJugador>();
            if (jugador != null)
            {
                StartCoroutine(AumentarVelocidad(jugador));
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 1f);
                Debug.Log("⚡ Velocidad aumentada!");
            }
        }
    }
    
    IEnumerator AumentarVelocidad(ControlJugador jugador)
    {
        float velocidadOriginal = jugador.velocidad;
        jugador.velocidad *= multiplicadorVelocidad;
        yield return new WaitForSeconds(duracion);
        jugador.velocidad = velocidadOriginal;
        Debug.Log("⚡ Velocidad normal");
    }
}