/* 
===========================================================
SPAWNER DE ENEMIGOS CON SONIDO
===========================================================
CONFIGURACION:
1. Crear un objeto vacío en la escena (llamalo "Spawner")
2. Arrastrar este script al objeto vacío
3. Crear un Prefab del enemigo (azul en Assets)
4. Crear puntos de spawn (objetos vacíos como "Punto1", "Punto2", etc.)
5. Agregar AudioSource al Spawner (o el script lo crea solo)
===========================================================
*/

using UnityEngine;

public class SpawnerConSonido : MonoBehaviour 
{
    [Header("Configuración de Spawn")]
    public GameObject enemigoPrefab;
    public float tiempoAparicion = 5f;
    public Transform[] puntosAparicion;
    
    [Header("Configuración de Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoAlNacer;
    public float volumenSonido = 0.7f;
    
    void Start() 
    {
        // Configurar AudioSource automáticamente
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null && sonidoAlNacer != null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (audioSource != null)
        {
            audioSource.spatialBlend = 0f; // Sonido 2D (se escucha global)
            audioSource.volume = volumenSonido;
        }
        
        // Iniciar el spawn
        InvokeRepeating("CrearEnemigo", 2f, tiempoAparicion);
    }
    
    void CrearEnemigo() 
    {
        if (enemigoPrefab != null && puntosAparicion.Length > 0)
        {
            // Elegir un punto al azar
            int indiceAleatorio = Random.Range(0, puntosAparicion.Length);
            Transform puntoElegido = puntosAparicion[indiceAleatorio];
            
            if (puntoElegido != null)
            {
                // Crear el enemigo
                GameObject nuevoEnemigo = Instantiate(enemigoPrefab, puntoElegido.position, Quaternion.identity);
                Debug.Log("¡Nuevo enemigo creado en: " + puntoElegido.name);
                
                // REPRODUCIR SONIDO AL NACER
                if (audioSource != null && sonidoAlNacer != null)
                {
                    audioSource.PlayOneShot(sonidoAlNacer, volumenSonido);
                    Debug.Log("🔊 Sonido de nacimiento reproducido");
                }
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Faltan asignar: enemigoPrefab o puntosAparicion");
        }
    }
}