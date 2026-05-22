using UnityEngine;

public class SpawnerConSonido : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public Transform[] puntosAparicion;
    public int maxEnemigos = 3;
    public AudioClip sonidoAlNacer;
    
    private int enemigosCreados = 0;
    private int enemigosMuertos = 0;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (enemigoPrefab == null)
        {
            Debug.LogError("❌ ERROR: Asigna el enemigoPrefab en el Inspector del Spawner!");
            return;
        }
        
        if (puntosAparicion == null || puntosAparicion.Length == 0)
        {
            Debug.Log("⚠️ Creando puntos de spawn automáticos...");
            puntosAparicion = new Transform[4];
            for (int i = 0; i < 4; i++)
            {
                GameObject punto = new GameObject($"SpawnPoint{i+1}");
                punto.transform.SetParent(transform);
                punto.transform.position = new Vector3(i * 5 - 7, 0, (i % 2 == 0 ? -6 : 6));
                puntosAparicion[i] = punto.transform;
            }
        }
        
        for (int i = 0; i < maxEnemigos; i++)
        {
            Invoke("CrearEnemigo", i * 0.5f);
        }
    }
    
    void CrearEnemigo()
    {
        if (enemigosCreados >= maxEnemigos) return;
        if (puntosAparicion.Length == 0) return;
        if (enemigoPrefab == null) return;
        
        int indice = Random.Range(0, puntosAparicion.Length);
        Transform punto = puntosAparicion[indice];
        
        GameObject nuevoEnemigo = Instantiate(enemigoPrefab, punto.position, Quaternion.identity);
        enemigosCreados++;
        
        if (sonidoAlNacer != null)
            audioSource.PlayOneShot(sonidoAlNacer);
        
        EnemigoPerseguidor enemigo = nuevoEnemigo.GetComponent<EnemigoPerseguidor>();
        if (enemigo != null)
        {
            enemigo.OnMuerte += () => {
                enemigosMuertos++;
                Debug.Log($"💀 Enemigos muertos: {enemigosMuertos}/{maxEnemigos}");
                GameManager gm = FindFirstObjectByType<GameManager>();
                if (gm != null) gm.EnemigoMuerto();
            };
        }
    }
}