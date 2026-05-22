using UnityEngine;

public class ActivadorPorPiso : MonoBehaviour
{
    public GameObject puente;

    void Start()
    {
        if (puente != null)
            puente.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puente != null)
                puente.SetActive(true);
        }
    }
}