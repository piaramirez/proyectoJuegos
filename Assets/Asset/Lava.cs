using UnityEngine;

public class Lava : MonoBehaviour
{
    public float dañoPorSegundo = 30f;
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VidaJugador vida = other.GetComponent<VidaJugador>();
            if (vida != null)
                vida.RecibirDano(dañoPorSegundo * Time.deltaTime);
        }
    }
}