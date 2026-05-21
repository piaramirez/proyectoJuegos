using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Paneles de UI")]
    public GameObject panelInstrucciones;
    public GameObject panelPausa;
    
    [Header("Textos en pantalla")]
    public Text textoMunicion;
    public Text textoVida;
    public Text textoObjetivo;
    public Text textoContadorEsqueletos;
    
    [Header("Variables del juego")]
    public int esqueletosParaMatar = 3;
    private int esqueletosMatados = 0;
    private bool tieneLlave = false;
    
    [Header("Referencias")]
    public GameObject puertaFinal;
    public GameObject llaveObjeto;
    
    void Start()
    {
        ActualizarUI();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (puertaFinal != null)
            puertaFinal.SetActive(false);
    }
    
    void Update()
    {
        // Pausa con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool pausado = panelPausa.activeSelf;
            panelPausa.SetActive(!pausado);
            
            if (!pausado)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
        }
        
        // Ocultar instrucciones después de 5 segundos
        if (Time.time > 5f && panelInstrucciones.activeSelf)
        {
            panelInstrucciones.SetActive(false);
        }
    }
    
    public void MatarEsqueleto()
    {
        esqueletosMatados++;
        ActualizarUI();
        
        if (esqueletosMatados >= esqueletosParaMatar && !tieneLlave)
        {
            RecibirLlave();
        }
    }
    
    public void RecibirLlave()
{
    tieneLlave = true;
    textoObjetivo.text = "✅ TIENES LA LLAVE! Ve a la puerta";
    
    if (puertaFinal != null)
        puertaFinal.SetActive(true);
}

public void AbrirPuerta()
{
    textoObjetivo.text = "🚪 PUERTA ABIERTA! Escapa!";
}

    
    public void ActualizarMunicion(int actual, int total)
    {
        textoMunicion.text = $"🔫 {actual} / {total}";
    }
    
    public void ActualizarVida(float vida, float vidaMax)
    {
        textoVida.text = $"❤️ {vida} / {vidaMax}";
    }
    
    void ActualizarUI()
    {
        textoContadorEsqueletos.text = $"💀 Esqueletos: {esqueletosMatados}/{esqueletosParaMatar}";
        textoObjetivo.text = "🗡️ Mata a los esqueletos para obtener la llave";
    }
    
    public bool TieneLlave() { return tieneLlave; }
}