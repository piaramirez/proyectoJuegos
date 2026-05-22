using UnityEngine;

public class Llave : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager ui = FindFirstObjectByType<UIManager>();
            if (ui != null)
            {
                ui.MatarEsqueleto(); // O crea RecibirLlave()
            }
            
            Destroy(gameObject);
        }
    }
}