using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPrincipal;
    public GameObject panelNiveles;
    public GameObject panelCreditos;
    public GameObject panelInfo;
    
    [Header("Botones")]
    public Button botonJugar;
    public Button botonCreditos;
    public Button botonInfo;
    public Button botonSalir;
    public Button botonNivel1;
    public Button botonNivel2;
    public Button botonVolver;
    
    void Start()
    {
        // Conectar botones automáticamente
        if (botonJugar != null) botonJugar.onClick.AddListener(MostrarPanelNiveles);
        if (botonCreditos != null) botonCreditos.onClick.AddListener(MostrarPanelCreditos);
        if (botonInfo != null) botonInfo.onClick.AddListener(MostrarPanelInfo);
        if (botonSalir != null) botonSalir.onClick.AddListener(SalirJuego);
        if (botonNivel1 != null) botonNivel1.onClick.AddListener(CargarNivel1);
        if (botonNivel2 != null) botonNivel2.onClick.AddListener(CargarNivel2);
        if (botonVolver != null) botonVolver.onClick.AddListener(VolverAlMenu);
        
        MostrarPanelPrincipal();
    }
    
    public void MostrarPanelPrincipal()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(true);
        if (panelNiveles != null) panelNiveles.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(false);
        if (panelInfo != null) panelInfo.SetActive(false);
        Debug.Log("✅ Panel Principal activo");
    }
    
    public void MostrarPanelNiveles()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelNiveles != null) panelNiveles.SetActive(true);
        if (panelCreditos != null) panelCreditos.SetActive(false);
        if (panelInfo != null) panelInfo.SetActive(false);
        Debug.Log("✅ Panel Niveles activo");
    }
    
    public void MostrarPanelCreditos()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelNiveles != null) panelNiveles.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(true);
        if (panelInfo != null) panelInfo.SetActive(false);
        Debug.Log("✅ Panel Creditos activo");
    }
    
    public void MostrarPanelInfo()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelNiveles != null) panelNiveles.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(false);
        if (panelInfo != null) panelInfo.SetActive(true);
        Debug.Log("✅ Panel Info activo");
    }
    
    public void VolverAlMenu()
    {
        MostrarPanelPrincipal();
        Debug.Log("✅ Volviendo al menu");
    }
    
    public void CargarNivel1()
    {
        Debug.Log("🔄 Cargando Nivel 1...");
        SceneManager.LoadScene("Nivel1");
    }
    
    public void CargarNivel2()
    {
        Debug.Log("🔄 Cargando Nivel 2...");
        SceneManager.LoadScene("Nivel2");
    }
    
    public void SalirJuego()
    {
        Debug.Log("🚪 Saliendo...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}