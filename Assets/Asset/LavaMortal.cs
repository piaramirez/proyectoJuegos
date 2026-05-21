using UnityEngine;

public class LavaMortal : MonoBehaviour
{
    public float dañoPorSegundo = 50f;
    public bool muerteInstantanea = true;
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (muerteInstantanea)
            {
                VidaJugador vida = other.GetComponent<VidaJugador>();
                if (vida != null)
                {
                    vida.RecibirDano(1000f); // Muerte instantánea
                }
            }
            else
            {
                VidaJugador vida = other.GetComponent<VidaJugador>();
                if (vida != null)
                {
                    vida.RecibirDano(dañoPorSegundo * Time.deltaTime);
                }
            }
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            VidaJugador vida = col.gameObject.GetComponent<VidaJugador>();
            if (vida != null && muerteInstantanea)
            {
                vida.RecibirDano(1000f);
            }
        }
    }
}