using UnityEngine;

public class PuenteMovil : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Vector3 posicionFinal;
    public float velocidad = 3f;
    public float tiempoEspera = 2f;

    private Vector3 posicionInicial;
    private Vector3 posicionObjetivo;
    private bool arrancar = false;
    private float temporizadorEspera;
    private bool esperando = false;

    void Start()
    {
        posicionInicial = transform.position;
        posicionObjetivo = posicionFinal;
    }

    public void IniciarMovimiento()
    {
        arrancar = true;
    }

    void Update()
    {
        if (!arrancar) return;

        if (esperando)
        {
            temporizadorEspera += Time.deltaTime;
            if (temporizadorEspera >= tiempoEspera)
            {
                esperando = false;
                temporizadorEspera = 0f;
            }
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidad * Time.deltaTime);

        if (transform.position == posicionObjetivo)
        {
            esperando = true;
            
            if (posicionObjetivo == posicionFinal)
            {
                posicionObjetivo = posicionInicial;
            }
            else
            {
                posicionObjetivo = posicionFinal;
            }
        }
    }

    // --- EL PARCHE MATEMÁTICO INVERSO PARA LA ESCALA ---
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. Lo hacemos hijo para que te arrastre en el viaje y no te caigas al vacío
            collision.gameObject.transform.SetParent(transform);

            // 2. Anulamos el aplastamiento dividiendo 1 entre la escala deformada del puente
            Vector3 escalaPuente = transform.localScale;
            collision.gameObject.transform.localScale = new Vector3(
                1f / escalaPuente.x,
                1f / escalaPuente.y,
                1f / escalaPuente.z
            );
            
            Debug.Log("<color=green>🍏 PUENTE: Jugador arriba con escala matemática corregida.</color>");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Al bajarte o saltar a la puerta verde, te desvinculas
            collision.gameObject.transform.SetParent(null);
            
            // Forzamos tu tamaño real del mundo
            collision.gameObject.transform.localScale = Vector3.one;
            
            Debug.Log("<color=green>🍏 PUENTE: Jugador abajo con escala normalizada.</color>");
        }
    }
}