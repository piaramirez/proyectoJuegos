using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public GameObject botonOculto; 
    public GameObject puente;
    public GameObject panelVictoria;
    public GameObject puertaFinal; // <-- Arrastra aquí la Puerta Final en Unity
    
    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI textoObjetivo;
    
    [Header("Configuración de Enemigos")]
    public int enemigosNecesarios = 3;
    private int enemigosMuertos = 0;
    private int pasoActual = 1;
    private bool juegoTerminado = false;
    private bool estaEnLava = false; 
    private bool velocidadActivada = false; 

    void Start()
    {
        Time.timeScale = 1f;
        
        if (botonOculto != null) botonOculto.SetActive(false);
        if (puente != null) puente.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        
        ActualizarObjetivo();
    }

    void Update()
    {
        if (juegoTerminado && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ReiniciarNivel();
        }
    }
    
    // 🍏 UNIFICADO: Esta es la función que llama el esqueleto al parpadear en rojo
    public void EnemigoMuerto()
    {
        enemigosMuertos++;
        
        // 🛠️ PARCHE DEL CONTADOR VISUAL: Actualiza directo el texto de la esquina
        ActualizarObjetivo();
        
        if (enemigosMuertos >= enemigosNecesarios && pasoActual == 1)
        {
            if (botonOculto != null)
            {
                botonOculto.SetActive(true); 
                pasoActual = 2;
                ActualizarObjetivo();
            }

            // 💥 ¡PARCHE DE LA PUERTA! En cuanto mueren los 3, cambia a color verde
            if (puertaFinal != null)
            {
                Renderer rend = puertaFinal.GetComponent<Renderer>();
                if (rend == null) rend = puertaFinal.GetComponentInChildren<Renderer>();
                
                if (rend != null)
                {
                    rend.material.color = Color.green;
                    Debug.Log("<color=green>🚪 PUERTA FINAL: ¡Cambió a color verde! Lista para recibir al jugador.</color>");
                }
            }
        }
    }
    
    // Esta función la llama el script del Botón cuando dejas caer la caja
    public void ActivarPuente()
    {
        if (pasoActual == 2)
        {
            // 🍏 SI DECIDISTE QUITAR LA LAVA EN LUGAR DE MOVER EL PUENTE:
            GameObject objetoLava = GameObject.Find("Lava");
            if (objetoLava != null)
            {
                objetoLava.SetActive(false); // Quita la lava directamente
                Debug.Log("🍏 ¡Lava eliminada por el botón!");
            }

            // Si prefieres mantener el puente físico:
            if (puente != null) puente.SetActive(true); 
            
            pasoActual = 3;
            
            if (textoObjetivo != null)
            {
                textoObjetivo.text = "OBJETIVO 3/3: ¡Sube a la plataforma y avanza!";
            }
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

    public void MostrarLetreroLava(bool enLava)
    {
        if (juegoTerminado) return; 
        
        estaEnLava = enLava;
        
        if (textoObjetivo != null)
        {
            if (estaEnLava)
            {
                textoObjetivo.text = "PELIGRO, TE QUEMAS"; 
            }
            else
            {
                ActualizarObjetivo(); 
            }
        }
    }

    public void MostrarLetreroVelocidad(bool activa)
    {
        if (juegoTerminado || estaEnLava) return;

        velocidadActivada = activa;

        if (textoObjetivo != null)
        {
            if (velocidadActivada)
            {
                textoObjetivo.text = "VELOCIDAD ACTIVADA";
            }
            else
            {
                ActualizarObjetivo();
            }
        }
    }

    public void TerminarPorDerrota()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (textoObjetivo != null)
        {
            textoObjetivo.text = "<color=red>¡HAS MUERTO!</color> Presiona [Enter] para volver a empezar";
        }
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ActualizarObjetivo()
    {
        if (textoObjetivo == null || juegoTerminado || estaEnLava || velocidadActivada) return;
        
        switch (pasoActual)
        {
            case 1: textoObjetivo.text = "OBJETIVO 1/3: Mata a los 3 esqueletos (" + enemigosMuertos + "/" + enemigosNecesarios + ")"; break;
            case 2: textoObjetivo.text = "OBJETIVO 2/3: Coloca la caja en el boton rojo"; break;
            case 3: textoObjetivo.text = "OBJETIVO 3/3: ¡Sube a la plataforma y avanza!"; break; 
            case 4: textoObjetivo.text = "FELICIDADES! NIVEL COMPLETADO"; break;
        }
    }
}