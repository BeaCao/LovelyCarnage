using UnityEngine;
using TMPro;
using UnityEngine.UI; // Necesario para acceder al componente Button y Image

public class ShopManager : MonoBehaviour
{
    // ==========================================
    // 1. CONFIGURACIÓN DE LA INTERFAZ (UI)
    // ==========================================
    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI tearsText; // Texto que muestra tus lágrimas totales
    public Button[] skinButtons;      // Arrastra tus 4 botones en orden (0, 1, 2, 3)

    // ==========================================
    // 2. CONFIGURACIÓN DE PRECIOS
    // ==========================================
    [Header("Ajustes de Precios")]
    // Definimos los precios: Skin 0 = 0, Skin 1 = 10, etc.
    public int[] preciosSkins = { 0, 10, 50, 100 }; 

    private int currentTears;

    // ----------------------------------------------------------------------------------

    void Start()
    {
        // Cargamos las lágrimas guardadas
        currentTears = PlayerPrefs.GetInt("SavedTears", 0);
        
        // Regla de oro: La skin inicial (0) siempre está marcada como comprada
        PlayerPrefs.SetInt("SkinComprada_0", 1); 
        
        // Refrescamos la tienda para que los botones muestren el estado correcto al abrirla
        UpdateUI();
    }

    // ==========================================
    // 3. FUNCIONES PARA LOS BOTONES
    // ==========================================
    // Asigna cada una de estas funciones al evento OnClick() de cada botón en Unity
    public void BuyDefaultSkin() { ProcessPurchase(0, preciosSkins[0]); }
    public void BuySkin1()       { ProcessPurchase(1, preciosSkins[1]); }
    public void BuySkin2()       { ProcessPurchase(2, preciosSkins[2]); }
    public void BuySkin3()       { ProcessPurchase(3, preciosSkins[3]); }

    // ==========================================
    // 4. LÓGICA DE COMPRA Y SELECCIÓN
    // ==========================================
    private void ProcessPurchase(int skinID, int price)
    {
        // Revisamos si el jugador ya es dueño de esta skin
        bool yaComprada = PlayerPrefs.GetInt("SkinComprada_" + skinID, 0) == 1;

        if (yaComprada)
        {
            // Si ya la tiene, simplemente la equipamos
            EquiparSkin(skinID);
        }
        else if (currentTears >= price)
        {
            // Si NO la tiene pero tiene dinero suficiente: COMPRA
            currentTears -= price;
            
            // Guardamos el nuevo saldo de lágrimas
            PlayerPrefs.SetInt("SavedTears", currentTears);
            
            // Guardamos que esta skin ya ha sido comprada permanentemente
            PlayerPrefs.SetInt("SkinComprada_" + skinID, 1);
            
            // La equipamos automáticamente tras comprarla
            EquiparSkin(skinID);
            
            Debug.Log("¡Compra realizada con éxito!");
        }
        else
        {
            // No hay dinero suficiente
            Debug.Log("Lágrimas insuficientes para comprar esta skin.");
        }
    }

    private void EquiparSkin(int skinID)
    {
        // Guardamos el ID de la skin que debe usar el Player en el juego
        PlayerPrefs.SetInt("SkinActiva", skinID);
        PlayerPrefs.Save(); // Guardado forzoso en el disco
        
        // Actualizamos la tienda para que cambien los textos de los botones
        UpdateUI();
    }

    // ==========================================
    // 5. ACTUALIZACIÓN VISUAL (LO QUE PIDES)
    // ==========================================
    void UpdateUI()
    {
        // Actualizamos el contador de dinero arriba
        if (tearsText != null)
        {
            tearsText.text = "Tears: " + currentTears;
        }

        // Obtenemos cuál es la skin que está puesta actualmente
        int skinActual = PlayerPrefs.GetInt("SkinActiva", 0);

        // Recorremos todos los botones para cambiar sus textos y colores
        for (int i = 0; i < skinButtons.Length; i++)
        {
            if (skinButtons[i] == null) continue;

            // Buscamos el componente de texto que está dentro del botón (hijo)
            TextMeshProUGUI textoBoton = skinButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            
            // Comprobamos si esta skin de la lista ya ha sido comprada
            bool comprada = PlayerPrefs.GetInt("SkinComprada_" + i, 0) == 1;

            // --- ESTADO 1: ES LA SKIN EQUIPADA ---
            if (i == skinActual)
            {
                textoBoton.text = "EQUIPADO";
                skinButtons[i].image.color = Color.green; // Color verde para saber qué llevas puesto
            }
            // --- ESTADO 2: YA ESTÁ COMPRADA PERO NO EQUIPADA ---
            else if (comprada)
            {
                textoBoton.text = "COMPRADO";
                skinButtons[i].image.color = Color.white; // Color blanco normal
            }
            // --- ESTADO 3: NO ESTÁ COMPRADA (Muestra el precio) ---
            else
            {
                textoBoton.text = preciosSkins[i] + " TEARS";
                skinButtons[i].image.color = Color.gray; // Color gris para indicar que está bloqueada
            }
        }
    }

    // Función para volver al Menú Principal
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}