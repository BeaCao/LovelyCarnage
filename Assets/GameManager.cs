using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas (Reiniciar/Menu)
using TMPro; // Necesario para usar los textos modernos (TextMeshPro)

public class GameManager : MonoBehaviour
{
    // ==========================================
    // 1. CONFIGURACIÓN DEL JUEGO
    // ==========================================
    [Header("Configuración General")]
    public float gameSpeed = 10f;       // Velocidad actual del juego
    public float acceleration = 0.05f;  // Cuánto aumenta la velocidad cada segundo
    
    public GameObject gameOverPanel;    // El panel negro de "Game Over"

    // ==========================================
    // 2. INTERFAZ DE USUARIO (HUD - DURANTE EL JUEGO)
    // ==========================================
    [Header("Interfaz (Jugando)")]
    public TextMeshProUGUI distanceText; // Texto de la esquina: "Distancia: 120m"
    public TextMeshProUGUI tearsText;    // Texto de la esquina: "Tears: 50"
    public GameObject newRecordObject;   // El aviso visual de "¡NUEVO RÉCORD!"

    // ==========================================
    // 3. INTERFAZ DE GAME OVER (AL MORIR)
    // ==========================================
    [Header("Interfaz (Pantalla Final)")]
    public TextMeshProUGUI finalDistanceText; // Texto dentro del panel: Distancia final
    public TextMeshProUGUI finalBestText;     // Texto dentro del panel: Mejor récord histórico

    // ==========================================
    // 4. VARIABLES INTERNAS (LÓGICA)
    // ==========================================
    private float distanceTraveled = 0f;  // Metros recorridos en esta partida
    private int totalTears = 0;           // Lágrimas totales (dinero)
    private float highScore = 0f;         // Récord máximo guardado en memoria
    private bool hasBeatenRecord = false; // ¿Hemos superado el récord en esta partida?

    // ==========================================
    // 5. SONIDO
    // ==========================================
    [Header("Audio")]
    public AudioClip deathSound;     // Archivo de sonido al morir
    private AudioSource audioSource; // El componente que emite el sonido

    // ----------------------------------------------------------------------------------

    void Start()
    {
        // 1. CARGAR DATOS GUARDADOS
        // Leemos la memoria del ordenador/móvil. Si no hay datos, usa 0 por defecto.
        totalTears = PlayerPrefs.GetInt("SavedTears", 0);
        highScore = PlayerPrefs.GetFloat("HighScore", 0);

        // 2. ACTUALIZAR PANTALLA
        UpdateUI(); // Pone los textos al día nada más empezar

        // 3. CONFIGURAR AUDIO
        // Buscamos si tenemos un "Altavoz" en este objeto. Si no, lo creamos.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 4. ESTADO INICIAL
        // Aseguramos que el aviso de "Nuevo Récord" esté apagado al empezar
        if (newRecordObject != null)
        {
            newRecordObject.SetActive(false);
        }
    }

    void Update()
    {
        // Solo ejecutamos la lógica si el juego NO está pausado (TimeScale no es 0)
        if (Time.timeScale != 0)
        {
            // A. AUMENTAR VELOCIDAD (ACELERACIÓN)
            // Time.deltaTime hace que funcione igual en ordenadores rápidos y lentos
            gameSpeed += acceleration * Time.deltaTime;

            // B. CALCULAR DISTANCIA
            // Fórmula física: Distancia = Velocidad * Tiempo
            distanceTraveled += gameSpeed * Time.deltaTime;

            // C. COMPROBAR RÉCORD EN TIEMPO REAL
            if (distanceTraveled > highScore)
            {
                // ¡Estamos batiendo el récord ahora mismo!
                highScore = distanceTraveled; 

                // Si es la primera vez que ocurre en esta partida, encendemos el aviso
                if (!hasBeatenRecord)
                {
                    hasBeatenRecord = true;
                    if (newRecordObject != null) newRecordObject.SetActive(true); // ¡FLASH!
                }
            }

            // D. ACTUALIZAR TEXTO DE DISTANCIA (HUD)
            if (distanceText != null)
            {
                // (int) quita los decimales para que se vea bonito (10m, no 10.432m)
                distanceText.text = "Distancia: " + (int)distanceTraveled + "m";
            }
        }
    }

    // --- FUNCIÓN PARA SUMAR LÁGRIMAS ---
    public void AddTears(int amount)
    {
        totalTears += amount;
        
        // Guardamos inmediatamente para que no se pierdan si el juego se cierra
        PlayerPrefs.SetInt("SavedTears", totalTears);
        PlayerPrefs.Save(); 
        
        UpdateUI();
    }

    // --- FUNCIÓN AUXILIAR PARA ACTUALIZAR TEXTOS ---
    void UpdateUI()
    {
        if (tearsText != null)
        {
            tearsText.text = "Tears: " + totalTears;
        }
    }

    // ==========================================
    // GAME OVER (LO QUE PASA AL MORIR)
    // ==========================================
    public void GameOver()
    {
        // 1. GUARDAR RÉCORD (Solo si lo hemos superado)
        if (hasBeatenRecord)
        {
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // 2. RELLENAR EL PANEL DE GAME OVER CON DATOS
        if (finalDistanceText != null)
        {
            finalDistanceText.text = "Distancia: " + (int)distanceTraveled + "m";
        }
        if (finalBestText != null)
        {
            finalBestText.text = "Mejor Récord: " + (int)highScore + "m";
        }

        // 3. SONIDO DE MUERTE
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // 4. MOSTRAR PANTALLA Y PARAR EL TIEMPO
        gameOverPanel.SetActive(true); 
        Time.timeScale = 0; // Esto congela todo el juego
    }

    // ==========================================
    // BOTONES DEL MENÚ
    // ==========================================
    public void RestartGame()
    {
        Time.timeScale = 1; // IMPORTANTE: Descongelar el tiempo antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void BackToMenu()
    {
        Time.timeScale = 1; // IMPORTANTE: Descongelar el tiempo antes de salir
        SceneManager.LoadScene("Menu"); // Asegúrate que tu escena se llama "Menu"
    }
}