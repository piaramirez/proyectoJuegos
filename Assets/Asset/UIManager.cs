using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelInstrucciones;
    public GameObject panelPausa;
    
    [Header("Textos")]
    public TMP_Text textoMunicion;
    public TMP_Text textoVida;
    public TMP_Text textoObjetivo;
    public TMP_Text textoContadorEsqueletos;
    
    [Header("Barra de Vida")]
    public Image barraVida;
    
    private VidaJugador vidaJugador;
    private PlayerDisparo playerDisparo;
    private int esqueletosMatados = 0;
    public int esqueletosParaMatar = 3;
    
    void Start()
    {
        vidaJugador = FindFirstObjectByType<VidaJugador>();
        playerDisparo = FindFirstObjectByType<PlayerDisparo>();
        
        ActualizarVida();
        ActualizarMunicion();
        
        if (panelInstrucciones != null)
            Invoke("OcultarInstrucciones", 5f);
        
        if (panelPausa != null)
            panelPausa.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool pausado = panelPausa.activeSelf;
            panelPausa.SetActive(!pausado);
            
            if (!pausado)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    
    void OcultarInstrucciones()
    {
        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);
    }
    
    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
    
    public void MatarEsqueleto()
    {
        esqueletosMatados++;
        if (textoContadorEsqueletos != null)
            textoContadorEsqueletos.text = $"💀 {esqueletosMatados}/{esqueletosParaMatar}";
        
        // Si ya mató suficientes, actualizar objetivo
        if (esqueletosMatados >= esqueletosParaMatar)
        {
            if (textoObjetivo != null)
                textoObjetivo.text = "✅ TIENES LA LLAVE! Ve a la puerta";
        }
    }
    
    public void ActualizarMunicion()
    {
        if (playerDisparo != null && textoMunicion != null)
            textoMunicion.text = $"🔫 {playerDisparo.balasEnCargador}/{playerDisparo.balasTotales}";
    }
    
    public void ActualizarVida()
    {
        if (vidaJugador != null)
        {
            float vidaActual = vidaJugador.GetVidaActual();
            float vidaMax = vidaJugador.GetVidaMaxima();
            
            if (textoVida != null)
                textoVida.text = $"❤️ {(int)vidaActual}/{(int)vidaMax}";
            
            if (barraVida != null)
                barraVida.fillAmount = vidaActual / vidaMax;
        }
    }
    
    public bool TieneLlave()
    {
        return esqueletosMatados >= esqueletosParaMatar;
    }
    
    public void AbrirPuerta()
    {
        if (textoObjetivo != null)
            textoObjetivo.text = "🚪 PUERTA ABIERTA! Escapa!";
    }
}