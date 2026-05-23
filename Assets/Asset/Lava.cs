using UnityEngine;

public class Lava : MonoBehaviour
{
    public float dañoPorSegundo = 30f;
    private float siguienteDaño;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));
            if (gm != null) gm.MostrarLetreroLava(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= siguienteDaño)
        {
            VidaJugador vida = other.GetComponent<VidaJugador>();
            if (vida == null) vida = other.GetComponentInChildren<VidaJugador>();

            if (vida != null)
            {
                vida.RecibirDano(dañoPorSegundo); 
                siguienteDaño = Time.time + 1f;   
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = (GameManager)FindObjectOfType(typeof(GameManager));
            if (gm != null) gm.MostrarLetreroLava(false);
        }
    }
}