using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 25f;
    public float tiempoVida = 3f;
    private Transform objetivo;

    void Start()
    {
        // 🔥 EL PARCHE DEFINITIVO: Buscar al jugador y ordenarle a Unity que ignore sus colisiones
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            Collider colJugador = jugador.GetComponent<Collider>();
            Collider miCol = GetComponent<Collider>();
            
            // Si el jugador o la bala tienen colliders en los hijos, también los ignoramos
            if (colJugador != null && miCol != null)
            {
                Physics.IgnoreCollision(miCol, colJugador, true);
            }
            
            // Por si acaso tu Hero_Ice tiene los colliders en el cuerpo hijo (Hero_Ice_Body)
            Collider[] collidersHijos = jugador.GetComponentsInChildren<Collider>();
            foreach (Collider c in collidersHijos)
            {
                if (miCol != null && c != null) Physics.IgnoreCollision(miCol, c, true);
            }
        }

        GameObject enemigo = GameObject.FindWithTag("Enemigo");
        if (enemigo != null) objetivo = enemigo.transform;
        
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        if (objetivo != null)
        {
            Vector3 centroEnemigo = objetivo.position + Vector3.up * 1f; // Apunta al pecho
            Vector3 direccion = (centroEnemigo - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;
            transform.forward = direccion;

            if (Vector3.Distance(transform.position, centroEnemigo) < 1.8f)
            {
                ImpactarEnemigo(objetivo.gameObject);
                return;
            }
        }
        else
        {
            transform.position += transform.forward * velocidad * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name.Contains("Hero")) return;
        
        if (other.gameObject.name.ToLower().Contains("piso") || 
            other.gameObject.name.ToLower().Contains("suelo") || 
            other.gameObject.name.ToLower().Contains("floor")) return;

        if (other.CompareTag("Enemigo") || other.gameObject.name.Contains("Skeleton"))
        {
            ImpactarEnemigo(other.gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    void ImpactarEnemigo(GameObject enemigo)
    {
        EnemigoPerseguidor enemigoScript = enemigo.GetComponent<EnemigoPerseguidor>();
        if (enemigoScript != null)
        {
            float dañoCalculado = enemigoScript.vidaEnemigo / 3f;
            enemigoScript.RecibirDaño(dañoCalculado + 1f); 
        }
        Destroy(gameObject); 
    }
}