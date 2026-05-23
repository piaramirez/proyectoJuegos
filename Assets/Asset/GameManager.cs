using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public GameObject botonOculto; 
    public GameObject puente;
    public GameObject panelVictoria;
    public GameObject puertaFinal; 
    
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
    
    public void EnemigoMuerto()
    {
        enemigosMuertos++;
        ActualizarObjetivo();
        
        if (enemigosMuertos >= enemigosNecesarios && pasoActual == 1)
        {
            if (botonOculto != null)
            {
                botonOculto.SetActive(true); 
                pasoActual = 2;
                ActualizarObjetivo();
            }

            if (puertaFinal != null)
            {
                Renderer rend = puertaFinal.GetComponent<Renderer>();
                if (rend == null) rend = puertaFinal.GetComponentInChildren<Renderer>();
                
                if (rend != null)
                {
                    rend.material.color = Color.green;
                    Debug.Log("<color=green>🚪 PUERTA FINAL: ¡Cambió a color verde!</color>");
                }
            }
        }
    }
    
    public void ActivarPuente()
    {
        if (pasoActual == 2)
        {
            GameObject objetoLava = GameObject.Find("Lava");
            if (objetoLava != null)
            {
                objetoLava.SetActive(false); 
                Debug.Log("🍏 Lava eliminada por el botón.");
            }

            if (puente != null) puente.SetActive(true); 
            
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

    // 🔥 PARCHE ROJO LAVA: Formato Rich Text activado
    public void MostrarLetreroLava(bool enLava)
    {
        if (juegoTerminado) return; 
        
        estaEnLava = enLava;
        
        if (textoObjetivo != null)
        {
            if (estaEnLava)
            {
                textoObjetivo.text = "<color=red><b>⚠️ ¡PELIGRO! TE ESTÁS QUEMANDO EN LA LAVA</b></color>"; 
            }
            else
            {
                ActualizarObjetivo(); 
            }
        }
    }

    // 🔥 PARCHE CYAN VELOCIDAD: Formato Rich Text con código Hexadecimal pro
    public void MostrarLetreroVelocidad(bool activa)
    {
        if (juegoTerminado || estaEnLava) return;

        velocidadActivada = activa;

        if (textoObjetivo != null)
        {
            if (velocidadActivada)
            {
                textoObjetivo.text = "<color=#00FFFFFF><b>⚡ ¡VELOCIDAD EXTRA ACTIVADA!</b></color>";
            }
            else
            {
                ActualizarObjetivo();
            }
        }
    }

    // 🔥 PARCHE VERDE CURACIÓN: Muestra el letrero y lo limpia automáticamente a los 1.5s
    public void MostrarLetreroCuracion()
    {
        if (juegoTerminado || estaEnLava) return;

        if (textoObjetivo != null)
        {
            textoObjetivo.text = "<color=green><b>💚 +30 DE VIDA RECOLECTADA</b></color>";
            CancelInvoke("RestaurarTextoNormal");
            Invoke("RestaurarTextoNormal", 1.5f);
        }
    }

    void RestaurarTextoNormal()
    {
        ActualizarObjetivo();
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
            textoObjetivo.text = "<color=red><b>¡HAS MUERTO!</b></color> Presiona [Enter] para reiniciar.";
        }
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActualizarObjetivo()
    {
        if (textoObjetivo == null || juegoTerminado || estaEnLava || velocidadActivada) return;
        
        switch (pasoActual)
        {
            case 1: textoObjetivo.text = "OBJETIVO 1/3: Mata a los 3 esqueletos (" + enemigosMuertos + "/" + enemigosNecesarios + ")"; break;
            case 2: textoObjetivo.text = "OBJETIVO 2/3: Coloca la caja en el boton verde"; break;
            case 3: textoObjetivo.text = "OBJETIVO 3/3: ¡Sube a la plataforma y avanza!"; break; 
            case 4: textoObjetivo.text = "NIVEL COMPLETADO"; break;
        }
    }
}