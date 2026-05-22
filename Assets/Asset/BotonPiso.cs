using UnityEngine;

public class BotonPiso : MonoBehaviour
{
    private bool presionado = false;

    void OnTriggerEnter(Collider other)
    {
        // Si entra la caja y no se ha presionado antes
        if (!presionado && (other.CompareTag("Caja") || other.gameObject.name.ToLower().Contains("caja")))
        {
            presionado = true;
            
            // 1. Busca el puente/plataforma y lo activa
            PuenteMovil puente = Object.FindFirstObjectByType<PuenteMovil>();
            if (puente != null)
            {
                puente.IniciarMovimiento();
            }

            // 2. PARCHE DE BLOQUEO TOTAL: Destruir la caja que cayó encima
            Destroy(other.gameObject);
            Debug.Log("<color=cyan>📦 CAJA: Desaparecida con éxito.</color>");

            // 3. DESAPARECER EL BOTÓN VERDE:
            // Apagamos su MeshRenderer y su Collider para que se vuelva invisible e intangible.
            // Así el jugador ya no puede chocar con él, ni verlo flotando, ni agarrarlo jamás.
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
            if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;

            Debug.Log("<color=green>🟢 BOTÓN: Desactivado y ocultado para evitar que lo agarren.</color>");
        }
    }
}