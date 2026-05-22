using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class ControlJugador : MonoBehaviour 
{
    [Header("Movimiento y Físicas")]
    public float velocidad = 6f;
    public float velocidadRotacion = 15f;
    public float fuerzaSalto = 7f;
    private Rigidbody rb;
    private Vector3 direccionMovimiento;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clipCaminar;
    public AudioClip clipSaltar;
    public float volumenCaminar = 0.5f;
    public float volumenSaltar = 1f;

    [Header("Sistema de Disparo e Interfaz")]
    public int balasEnCargador = 15;
    public int capacidadCargador = 15;
    public int balasTotales = 999; // ¡MUNICIÓN INFINITA PARA TESTEO!
    public float tiempoEntreDisparos = 0.2f;
    private float tiempoSiguienteDisparo;
    
    public GameObject prefabBala;
    public Transform puntoDisparo; 
    public TextMeshProUGUI textoMunicionUI; 

    [Header("Sonidos de Armas")]
    public AudioClip sonidoDisparo;
    public AudioClip sonidoSinBalas;
    public AudioClip sonidoRecargar;
    
    [Header("Sistema de Agarre (Radar Proximidad)")]
    public Transform puntoAgarre; 
    public float radioAgarre = 3.5f;
    public KeyCode teclaAgarrar = KeyCode.E;
    private GameObject objetoEnMano = null;
    private TextMeshProUGUI textoCentroUI;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        
        audioSource.spatialBlend = 0f;
        
        ActualizarTextoBalas();

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas != null)
        {
            GameObject txtObj = new GameObject("TxtCentroDinamico");
            txtObj.transform.SetParent(canvas.transform);
            textoCentroUI = txtObj.AddComponent<TextMeshProUGUI>();
            textoCentroUI.fontSize = 28;
            textoCentroUI.color = Color.yellow;
            textoCentroUI.alignment = TextAlignmentOptions.Center;
            RectTransform rect = textoCentroUI.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 150);
            rect.sizeDelta = new Vector2(500, 50);
            textoCentroUI.gameObject.SetActive(false);
        }
    }
    
    void Update() 
    {
        float movH = Input.GetAxis("Horizontal");
        float movV = Input.GetAxis("Vertical");
        direccionMovimiento = new Vector3(movH, 0, movV).normalized;
        
        if (clipCaminar != null)
        {
            bool estaEnSuelo = Mathf.Abs(rb.linearVelocity.y) < 0.1f;
            bool moviendose = direccionMovimiento.magnitude > 0.1f;
            
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
        
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.linearVelocity.y) < 0.1f) 
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            if (clipSaltar != null)
                audioSource.PlayOneShot(clipSaltar, volumenSaltar);
        }
        
        if (Input.GetButton("Fire1") && Time.time >= tiempoSiguienteDisparo)
        {
            if (balasEnCargador > 0)
            {
                Disparar();
                tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
            }
            else if (Input.GetButtonDown("Fire1") && sonidoSinBalas != null)
            {
                audioSource.PlayOneShot(sonidoSinBalas, 0.6f);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Recargar();
        }

        if (Input.GetKeyDown(teclaAgarrar))
        {
            if (objetoEnMano == null)
                IntentarAgarrar();
            else
                SoltarObjeto();
        }
        
        ManejarTextoCentralDinamico();
    }
    
    void FixedUpdate()
    {
        if (direccionMovimiento.magnitude > 0.1f)
        {
            Vector3 velocidadTarget = direccionMovimiento * velocidad;
            velocidadTarget.y = rb.linearVelocity.y;
            rb.linearVelocity = velocidadTarget;

            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.fixedDeltaTime));
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    void Disparar()
    {
        balasEnCargador--;
        ActualizarTextoBalas();

        if (sonidoDisparo != null) audioSource.PlayOneShot(sonidoDisparo, 0.7f);

        if (puntoDisparo != null && prefabBala != null)
        {
            Vector3 posicionSegura = puntoDisparo.position + (puntoDisparo.forward * 1.2f); // Más salido al frente
            Instantiate(prefabBala, posicionSegura, puntoDisparo.rotation);
        }
    }

    void Recargar()
    {
        if (balasEnCargador == capacidadCargador || balasTotales <= 0) return;

        if (sonidoRecargar != null) audioSource.PlayOneShot(sonidoRecargar, 0.8f);

        int deLaBolsa = capacidadCargador - balasEnCargador;
        int inyectar = Mathf.Min(deLaBolsa, balasTotales);

        balasEnCargador += inyectar;
        balasTotales -= inyectar;
        
        ActualizarTextoBalas();
    }

    void ActualizarTextoBalas()
    {
        if (textoMunicionUI != null)
        {
            textoMunicionUI.text = "Balas: " + balasEnCargador + " / " + balasTotales;
        }
    }
    
    void IntentarAgarrar()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radioAgarre);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Recogible") || col.CompareTag("Caja"))
            {
                objetoEnMano = col.gameObject;
                Rigidbody objRb = objetoEnMano.GetComponent<Rigidbody>();
                if (objRb != null)
                {
                    objRb.isKinematic = true;
                    objRb.useGravity = false;
                }
                
                if (puntoAgarre != null)
                {
                    objetoEnMano.transform.SetParent(puntoAgarre);
                    objetoEnMano.transform.localPosition = Vector3.zero;
                }
                else
                {
                    objetoEnMano.transform.SetParent(transform);
                    objetoEnMano.transform.localPosition = new Vector3(0f, 0.5f, 2f);
                }
                objetoEnMano.transform.localRotation = Quaternion.identity;
                break;
            }
        }
    }
    
    void SoltarObjeto()
    {
        if (objetoEnMano != null)
        {
            Rigidbody objRb = objetoEnMano.GetComponent<Rigidbody>();
            if (objRb != null)
            {
                objRb.isKinematic = false;
                objRb.useGravity = true;
            }
            objetoEnMano.transform.SetParent(null);
            objetoEnMano = null;
        }
    }
    
    void ManejarTextoCentralDinamico()
    {
        if (textoCentroUI == null) return;

        if (balasEnCargador <= 0 && balasTotales > 0)
        {
            textoCentroUI.text = "¡SIN BALAS! PRESIONA [R] PARA RECARGAR";
            textoCentroUI.color = Color.red;
            textoCentroUI.gameObject.SetActive(true);
            return;
        }

        if (objetoEnMano != null)
        {
            textoCentroUI.gameObject.SetActive(false);
            return;
        }

        bool cercaDeCaja = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radioAgarre);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Recogible") || col.CompareTag("Caja"))
            {
                cercaDeCaja = true;
                break;
            }
        }

        if (cercaDeCaja)
        {
            textoCentroUI.text = "PRESIONA [E] PARA AGARRAR LA CAJA";
            textoCentroUI.color = Color.yellow;
            textoCentroUI.gameObject.SetActive(true);
        }
        else
        {
            textoCentroUI.gameObject.SetActive(false);
        }
    }
}