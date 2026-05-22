using UnityEngine;

public class Lava : MonoBehaviour
{
    public float dañoPorSegundo = 30f;
    private float siguienteDaño;

    // 1. Al pisar la lava: Mandamos el letrero rojo a la pantalla
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                gm.MostrarLetreroLava(true); // Prende el aviso en el objetivo global
            }
        }
    }

    // 2. Mientras te quedes parado: Te quita vida en golpe seco cada 1 segundo exacto
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= siguienteDaño)
        {
            VidaJugador vida = other.GetComponent<VidaJugador>();
            if (vida != null)
            {
                vida.RecibirDano(dañoPorSegundo); // Quita los 30 directos
                siguienteDaño = Time.time + 1f;   // Cooldown de 1 segundo para el próximo golpe
            }
        }
    }

    // 3. Al salir o saltar de la lava: Borramos el aviso y regresa tu objetivo normal
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                gm.MostrarLetreroLava(false); // Apaga el aviso de peligro
            }
        }
    }
}