using UnityEngine;

public class PlayerAgarrar : MonoBehaviour
{
    [Header("Configuración")]
    public float radioAlcance = 3.5f; // Distancia para detectar la caja
    private GameObject objetoAgarrado;
    private Transform puntoMano;

    void Start()
    {
        // Creamos el punto donde flotará la caja frente a ti
        GameObject manoObj = new GameObject("PuntoMano");
        manoObj.transform.SetParent(transform);
        // Ajusta el último número (2.2f) si quieres la caja más cerca o lejos de tu cuerpo
        manoObj.transform.localPosition = new Vector3(0f, -0.5f, 2.2f); 
        puntoMano = manoObj.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoAgarrado == null)
            {
                // Sistema de radar: detecta todo lo que esté cerca
                Collider[] colliders = Physics.OverlapSphere(transform.position, radioAlcance);
                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("Caja"))
                    {
                        objetoAgarrado = col.gameObject;
                        
                        Rigidbody rb = objetoAgarrado.GetComponent<Rigidbody>();
                        if (rb != null) rb.isKinematic = true; // Desactiva físicas para que no pese
                        
                        objetoAgarrado.transform.SetParent(puntoMano);
                        objetoAgarrado.transform.localPosition = Vector3.zero;
                        objetoAgarrado.transform.localRotation = Quaternion.identity;
                        break; // Bloquea la primera caja que encuentre
                    }
                }
            }
            else
            {
                // Soltar la caja de inmediato
                Rigidbody rb = objetoAgarrado.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;
                
                objetoAgarrado.transform.SetParent(null);
                objetoAgarrado = null;
            }
        }
    }
}