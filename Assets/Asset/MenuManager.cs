using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles de UI")]
    public GameObject panelPrincipal;
    public GameObject panelNiveles;
    public GameObject panelCreditos;
    public GameObject panelInfo;
    
    [Header("Botones de Nivel (opcional - arrastra desde la UI)")]
    public Button btnNivel1;
    public Button btnNivel2;
    public Button btnSalir;
    
    [Header("Información del Alumno")]
    public string nombreAlumno = "Tu Nombre";
    public string carrera = "Ingeniería en...";
    public string facultad = "Facultad de...";
    public string matricula = "TU-MATRICULA";
    
    [Header("Audios - ESPACIO PARA SONIDOS")]
    public AudioClip sonidoClick;
    public AudioClip sonidoHover;
    private AudioSource audioSource;
    
    void Start()
    {
        // Mostrar solo el panel principal al inicio
        MostrarPanelPrincipal();
        
        // Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Configurar botones si existen
        if (btnNivel1 != null)
            btnNivel1.onClick.AddListener(() => CargarNivel(1));
        
        if (btnNivel2 != null)
            btnNivel2.onClick.AddListener(() => CargarNivel(2));
        
        if (btnSalir != null)
            btnSalir.onClick.AddListener(SalirJuego);
        
        Debug.Log("✅ Menú Manager iniciado correctamente");
    }
    
    public void MostrarPanelPrincipal()
    {
        panelPrincipal.SetActive(true);
        panelNiveles.SetActive(false);
        panelCreditos.SetActive(false);
        panelInfo.SetActive(false);
    }
    
    public void MostrarPanelNiveles()
    {
        ReproducirClick();
        panelPrincipal.SetActive(false);
        panelNiveles.SetActive(true);
        panelCreditos.SetActive(false);
        panelInfo.SetActive(false);
    }
    
    public void MostrarPanelCreditos()
    {
        ReproducirClick();
        panelPrincipal.SetActive(false);
        panelNiveles.SetActive(false);
        panelCreditos.SetActive(true);
        panelInfo.SetActive(false);
    }
    
    public void MostrarPanelInfo()
    {
        ReproducirClick();
        panelPrincipal.SetActive(false);
        panelNiveles.SetActive(false);
        panelCreditos.SetActive(false);
        panelInfo.SetActive(true);
        
        // Actualizar textos de información (si tienes Text en el panel)
        ActualizarTextosInfo();
    }
    
    void ActualizarTextosInfo()
    {
        // Buscar textos por nombre y actualizar (opcional)
        Text txtNombre = GameObject.Find("TxtNombre")?.GetComponent<Text>();
        Text txtCarrera = GameObject.Find("TxtCarrera")?.GetComponent<Text>();
        Text txtFacultad = GameObject.Find("TxtFacultad")?.GetComponent<Text>();
        Text txtMatricula = GameObject.Find("TxtMatricula")?.GetComponent<Text>();
        
        if (txtNombre != null) txtNombre.text = $"Nombre: {nombreAlumno}";
        if (txtCarrera != null) txtCarrera.text = $"Carrera: {carrera}";
        if (txtFacultad != null) txtFacultad.text = $"Facultad: {facultad}";
        if (txtMatricula != null) txtMatricula.text = $"Matrícula: {matricula}";
    }
    
    public void CargarNivel(int nivel)
    {
        ReproducirClick();
        Debug.Log($"🔄 Cargando nivel {nivel}...");
        
        // Guardar qué nivel estamos jugando (para el GameManager)
        PlayerPrefs.SetInt("NivelActual", nivel);
        PlayerPrefs.Save();
        
        // Cargar la escena del nivel
        string nombreEscena = $"Nivel{nivel}";
        
        // Verificar si la escena existe en el build
        SceneManager.LoadScene(nombreEscena);
    }
    
    public void VolverAlMenu()
    {
        ReproducirClick();
        MostrarPanelPrincipal();
    }
    
    public void SalirJuego()
    {
        ReproducirClick();
        Debug.Log("🚪 Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    void ReproducirClick()
    {
        if (sonidoClick != null && audioSource != null)
            audioSource.PlayOneShot(sonidoClick, 0.7f);
    }
    
    public void ReproducirHover()
    {
        if (sonidoHover != null && audioSource != null)
            audioSource.PlayOneShot(sonidoHover, 0.5f);
    }
}