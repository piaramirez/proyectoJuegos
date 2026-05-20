using UnityEngine;

public class WeightPowerUp : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float duracion = 5f;
    [SerializeField] private GameObject efectoVisual;
    [SerializeField] private AudioClip sonidoRecoger;
    
    private AudioSource audioSource;
    private bool usado = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (usado) return;
        
        if (other.CompareTag("Player"))
        {
            WeightSystem weightSystem = other.GetComponent<WeightSystem>();
            
            if (weightSystem != null)
            {
                weightSystem.ActivarPowerUp();
                usado = true;
                
                if (sonidoRecoger != null)
                    audioSource.PlayOneShot(sonidoRecoger, 1f);
                
                if (efectoVisual != null)
                    Instantiate(efectoVisual, transform.position, Quaternion.identity);
                
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                
                Destroy(gameObject, 2f);
            }
        }
    }
}