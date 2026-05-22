using UnityEngine;

public class ObjetoRecogible : MonoBehaviour
{
    public float peso = 2f;
    
    private Rigidbody rb;
    private bool agarrado = false;
    private Transform puntoAgarre;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = peso;
        rb.useGravity = true;
        
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
            puntoAgarre = jugador.transform.Find("PuntoAgarre");
        
        gameObject.tag = "Recogible";
        Debug.Log("📦 Caja lista! Peso: " + peso);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !agarrado)
        {
            Agarrar();
        }
    }
    
    void Agarrar()
    {
        agarrado = true;
        rb.isKinematic = true;
        if (puntoAgarre != null)
        {
            transform.SetParent(puntoAgarre);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        Debug.Log("📦 Caja agarrada! Presiona E para soltar");
    }
    
    void Update()
    {
        if (agarrado && Input.GetKeyDown(KeyCode.E))
        {
            Soltar();
        }
    }
    
    void Soltar()
    {
        agarrado = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        Debug.Log("📦 Caja soltada!");
    }
}