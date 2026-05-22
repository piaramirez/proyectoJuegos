using UnityEngine;
using TMPro;

public class ControlJugador : MonoBehaviour 
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    private Rigidbody rb;
    
    public AudioSource audioSource;
    public AudioClip clipCaminar;
    public AudioClip clipSaltar;
    public float volumenCaminar = 0.5f;
    public float volumenSaltar = 1f;
    
    public Transform puntoAgarre;
    public float distanciaAgarre = 3f;
    public LayerMask capaRecogible;
    public KeyCode teclaAgarrar = KeyCode.E;
    private GameObject objetoEnMano = null;
    private TextMeshProUGUI textoRecogerUI;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        
        audioSource.spatialBlend = 0f;
        
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas != null)
        {
            GameObject txtObj = new GameObject("TxtRecoger");
            txtObj.transform.SetParent(canvas.transform);
            textoRecogerUI = txtObj.AddComponent<TextMeshProUGUI>();
            textoRecogerUI.fontSize = 28;
            textoRecogerUI.color = Color.yellow;
            textoRecogerUI.alignment = TextAlignmentOptions.Center;
            RectTransform rect = textoRecogerUI.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 150);
            rect.sizeDelta = new Vector2(400, 50);
            textoRecogerUI.gameObject.SetActive(false);
        }
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
                audioSource.PlayOneShot(clipSaltar, volumenSaltar);
        }
        
        if (Input.GetKeyDown(teclaAgarrar))
        {
            if (objetoEnMano == null)
                IntentarAgarrar();
            else
                SoltarObjeto();
        }
        
        MostrarTextoRecoger();
    }
    
    void IntentarAgarrar()
    {
        RaycastHit hit;
        Ray rayo = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (Physics.Raycast(rayo, out hit, distanciaAgarre, capaRecogible))
        {
            if (hit.collider.CompareTag("Recogible"))
            {
                objetoEnMano = hit.collider.gameObject;
                Rigidbody rb = objetoEnMano.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                
                objetoEnMano.transform.SetParent(puntoAgarre);
                objetoEnMano.transform.localPosition = Vector3.zero;
                objetoEnMano.transform.localRotation = Quaternion.identity;
            }
        }
    }
    
    void SoltarObjeto()
    {
        if (objetoEnMano != null)
        {
            Rigidbody rb = objetoEnMano.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;
            objetoEnMano.transform.SetParent(null);
            objetoEnMano = null;
        }
    }
    
    void MostrarTextoRecoger()
    {
        if (objetoEnMano != null) return;
        
        RaycastHit hit;
        Ray rayo = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (Physics.Raycast(rayo, out hit, distanciaAgarre, capaRecogible))
        {
            if (textoRecogerUI != null)
                textoRecogerUI.gameObject.SetActive(true);
        }
        else
        {
            if (textoRecogerUI != null)
                textoRecogerUI.gameObject.SetActive(false);
        }
    }
}