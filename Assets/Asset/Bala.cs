using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 25f;
    public float tiempoVida = 3f;

    void Start()
    {
        Debug.Log($"🚀 [Bala] Proyectil creado en Y fija: {transform.position.y}. Dirección: {transform.forward}");

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            Collider miCol = GetComponent<Collider>();
            Collider[] collidersHijos = jugador.GetComponentsInChildren<Collider>();
            foreach (Collider c in collidersHijos)
            {
                if (miCol != null && c != null) Physics.IgnoreCollision(miCol, c, true);
            }
        }
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.position += transform.forward * velocidad * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        EvaluarImpacto(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        EvaluarImpacto(collision.gameObject);
    }

    void EvaluarImpacto(GameObject objetoTocado)
    {
        if (objetoTocado.CompareTag("Player") || objetoTocado.name.Contains("Hero")) return;
        
        // 🔥 PARCHE DE OBSTÁCULOS: Si toca la lava, cajas o powerups, los atraviesa sin destruirse
        if (objetoTocado.name.ToLower().Contains("lava") || 
            objetoTocado.name.ToLower().Contains("caja") || 
            objetoTocado.name.ToLower().Contains("powerup"))
        {
            // Retornamos sin destruir la bala para que los atraviese
            return; 
        }

        // Si choca contra paredes o el piso firme del mapa, ahí sí se destruye
        if (objetoTocado.name.ToLower().Contains("piso") || objetoTocado.name.ToLower().Contains("suelo") || 
            objetoTocado.name.ToLower().Contains("wall") || objetoTocado.name.ToLower().Contains("pared"))
        {
            Debug.Log($"🧱 [Bala] Choque con estructura firme: '{objetoTocado.name}'. Destruyendo proyectil.");
            Destroy(gameObject);
            return;
        }

        // Detección del enemigo en cualquier parte de su modelo
        EnemigoPerseguidor enemigoScript = objetoTocado.GetComponent<EnemigoPerseguidor>();
        if (enemigoScript == null) enemigoScript = objetoTocado.GetComponentInParent<EnemigoPerseguidor>();
        if (enemigoScript == null) enemigoScript = objetoTocado.GetComponentInChildren<EnemigoPerseguidor>();

        if (objetoTocado.CompareTag("Enemigo") || objetoTocado.name.Contains("Skeleton") || enemigoScript != null)
        {
            if (enemigoScript != null)
            {
                Debug.Log($"🎯 [Bala] ¡GOLPE DIRECTO A: '{objetoTocado.name}'! Aplicando daño y retroceso.");
                enemigoScript.RecibirImpactoBala(transform.forward); 
            }
            
            Destroy(gameObject);
        }
    }
}