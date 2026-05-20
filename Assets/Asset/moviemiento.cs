using UnityEngine;

public class ControlJugador : MonoBehaviour 
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    private Rigidbody rb;
    
    public AudioSource audioSource;
    public AudioClip clipCaminar;
    public AudioClip clipSaltar;
    
    [Header("Volúmenes")]
    public float volumenCaminar = 0.5f;
    public float volumenSaltar = 1f;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        
        audioSource.spatialBlend = 0f;
    }
    
    void Update() 
    {
        float movH = Input.GetAxis("Horizontal");
        float movV = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movH, 0, movV);
        transform.Translate(movimiento * velocidad * Time.deltaTime);

    if (clipCaminar != null)
    {
        bool estaEnSuelo = Mathf.Abs(rb.linearVelocity.y) < 0.1f;
        bool moviendose = movimiento.magnitude > 0.1f;

        if (estaEnSuelo && moviendose)
        {
            if (!audioSource.isPlaying || audioSource.clip != clipCaminar)
            {
                audioSource.clip = clipCaminar;
                audioSource.loop = true;
                audioSource.volume = volumenCaminar;
                audioSource.Play();
            }
        }
        else if (audioSource.clip == clipCaminar)
        {
            audioSource.Stop();
        }
    }

    if(Input.GetButtonDown("Jump") && Mathf.Abs(rb.linearVelocity.y) < 0.1f) 
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        
        if (clipSaltar != null)
        {
            // PlayOneShot permite que suene aunque el caminar se detenga
            audioSource.PlayOneShot(clipSaltar, volumenSaltar);
        }
    }
    }
}