using UnityEngine;
using UnityEngine.UI; // para el botón

public class PuenteController : MonoBehaviour
{
    public GameObject puente;  // Arrastra aquí el objeto "Puente" en el inspector
    public Button botonPuente; // Arrastra aquí el botón "BotonPeso"
    public bool puenteActivo = false; // si empieza oculto o no

    void Start()
    {
        // Al inicio, ocultamos o mostramos el puente según puenteActivo
        if (puente != null)
            puente.SetActive(puenteActivo);

        // Asignamos la función al botón (si el botón está asignado)
        if (botonPuente != null)
            botonPuente.onClick.AddListener(TogglePuente);
    }

    void TogglePuente()
    {
        if (puente != null)
        {
            puenteActivo = !puenteActivo;
            puente.SetActive(puenteActivo);
            Debug.Log("Puente " + (puenteActivo ? "activado" : "desactivado"));
        }
    }
}