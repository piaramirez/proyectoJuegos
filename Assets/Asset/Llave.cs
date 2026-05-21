using UnityEngine;

public class Llave : MonoBehaviour
{
    public float rotacionVelocidad = 50f;
    public AudioClip sonidoRecoger;
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    void Update()
    {
        transform.Rotate(Vector3.up, rotacionVelocidad * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sonidoRecoger != null)
                audioSource.PlayOneShot(sonidoRecoger);
            
            // Buscar el UIManager
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
                ui.RecibirLlave();
            
            // Desactivar visualmente
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            
            Destroy(gameObject, 2f);
        }
    }
}