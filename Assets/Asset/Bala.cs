using UnityEngine;

public class Bala : MonoBehaviour
{
    public float daño = 25f;
    public float velocidad = 25f;
    public float tiempoVida = 3f;
    private Transform objetivo;

    void Start()
    {
        // Busca al enemigo más cercano al aparecer
        GameObject enemigo = GameObject.FindWithTag("Enemigo");
        if (enemigo != null) objetivo = enemigo.transform;
        
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        if (objetivo != null)
        {
            // Perseguir al objetivo
            Vector3 direccion = (objetivo.position - transform.position).normalized;
            Quaternion rotacionHacia = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionHacia, Time.deltaTime * 10f);
        }
        
        // Movimiento constante hacia adelante
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("Enemigo"))
        {
            EnemigoPerseguidor enemigoScript = other.GetComponent<EnemigoPerseguidor>();
            if (enemigoScript != null)
            {
                enemigoScript.RecibirDaño(daño);
            }
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}