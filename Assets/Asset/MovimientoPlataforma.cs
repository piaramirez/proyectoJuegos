using UnityEngine;

public class MovimientoPlataforma : MonoBehaviour
{
    public Transform Mov1;
    public Transform Mov2;
    public Transform Mov3;
    public float velocidad = 3f;
    private Transform destinoActual;
    private int indiceSiguiente = 1;
    
    [Header("Configuración de Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoMovimiento;
    public AudioClip sonidoLlegada;
    public AudioClip sonidoPisada;
    
    private bool reproduciendoMovimiento = false;
    
    void Start()
    {
        if (Mov1 != null)
        {
            transform.position = Mov1.position;
            destinoActual = Mov1;
        }
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null && (sonidoMovimiento != null || sonidoLlegada != null))
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (audioSource != null)
        {
            audioSource.spatialBlend = 1.0f;
            audioSource.loop = false;
        }
    }

    void Update()
    {
        if (Mov1 == null || Mov2 == null || Mov3 == null) return;

        transform.position = Vector3.MoveTowards(transform.position, destinoActual.position, velocidad * Time.deltaTime);
        
        bool enMovimiento = Vector3.Distance(transform.position, destinoActual.position) > 0.1f;
        
        if (enMovimiento && sonidoMovimiento != null)
        {
            if (!reproduciendoMovimiento)
            {
                audioSource.clip = sonidoMovimiento;
                audioSource.loop = true;
                audioSource.Play();
                reproduciendoMovimiento = true;
            }
        }
        else if (!enMovimiento && reproduciendoMovimiento)
        {
            audioSource.Stop();
            reproduciendoMovimiento = false;
            
            if (sonidoLlegada != null)
            {
                audioSource.PlayOneShot(sonidoLlegada, 0.7f);
            }
        }
        
        if (Vector3.Distance(transform.position, destinoActual.position) < 0.1f)
        {
            ActualizarDestino();
        }
    }

    void ActualizarDestino()
    {
        indiceSiguiente++;
        if (indiceSiguiente > 3) indiceSiguiente = 1;

        if (indiceSiguiente == 1) destinoActual = Mov1;
        else if (indiceSiguiente == 2) destinoActual = Mov2;
        else if (indiceSiguiente == 3) destinoActual = Mov3;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.SetParent(transform);
            
            if (sonidoPisada != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoPisada, 0.5f);
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.SetParent(null);
        }
    }
}