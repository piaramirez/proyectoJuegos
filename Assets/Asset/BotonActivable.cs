using UnityEngine;

public class BotonActivable : MonoBehaviour
{
    public GameObject objetoActivar;  // Puente, puerta, etc.
    public float pesoRequerido = 1f;
    public Color colorActivado = Color.green;
    public AudioClip sonidoActivar;
    
    private Renderer rend;
    private AudioSource audioSource;
    private bool activado = false;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        rend.material.color = Color.red;
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;
            rend.material.color = colorActivado;
            if (objetoActivar != null)
                objetoActivar.SetActive(!objetoActivar.activeSelf);
            if (sonidoActivar != null)
                audioSource.PlayOneShot(sonidoActivar);
            Debug.Log("✅ Botón activado! Puente/objeto cambiado");
        }
    }
}