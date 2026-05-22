using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public GameObject botonOculto; // <-- Aquí vas a arrastrar el BotonPeso en Unity
    public GameObject puente;
    public GameObject panelVictoria;
    
    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI textoObjetivo;
    
    [Header("Configuración de Enemigos")]
    public int enemigosNecesarios = 3;
    private int enemigosMuertos = 0;
    private int pasoActual = 1;
    
    void Start()
    {
        // Al empezar el juego obligamos a que el botón y el puente estén apagados
        if (botonOculto != null) botonOculto.SetActive(false);
        if (puente != null) puente.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        
        ActualizarObjetivo();
    }
    
    // ESTA ES LA FUNCIÓN QUE LLAMA TU ESQUELETO AL MORIR
    public void EnemigoMuerto()
    {
        enemigosMuertos++;
        Debug.Log($"Enemigo registrado por el GM. Cuenta: {enemigosMuertos}/{enemigosNecesarios}");
        
        // Si ya mataste los 3 esqueletos y sigues en el paso 1...
        if (enemigosMuertos >= enemigosNecesarios && pasoActual == 1)
        {
            if (botonOculto != null)
            {
                botonOculto.SetActive(true); // 💥 ¡AQUÍ ES DONDE SE ACTIVA TU BOTÓN EN EL MAPA!
                pasoActual = 2;
                ActualizarObjetivo();
                Debug.Log("¡El botón ha aparecido en el suelo!");
            }
        }
    }
    
    public void ActivarPuente()
    {
        if (pasoActual == 2)
        {
            if (puente != null) puente.SetActive(true); // 🌉 ¡AQUÍ APARECE EL PUENTE!
            pasoActual = 3;
            ActualizarObjetivo();
        }
    }
    
    public void CompletarNivel()
    {
        pasoActual = 4;
        ActualizarObjetivo();
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    void ActualizarObjetivo()
    {
        if (textoObjetivo == null) return;
        
        switch (pasoActual)
        {
            // Quitamos los emojis para evitar los cuadros blancos [□] de TMPro
            case 1: textoObjetivo.text = "OBJETIVO 1/3: Mata a los 3 esqueletos"; break;
            case 2: textoObjetivo.text = "OBJETIVO 2/3: Coloca la caja en el boton rojo"; break;
            case 3: textoObjetivo.text = "OBJETIVO 3/3: Cruza el puente y toca la puerta"; break;
            case 4: textoObjetivo.text = "FELICIDADES! NIVEL COMPLETADO"; break;
        }
    }
}