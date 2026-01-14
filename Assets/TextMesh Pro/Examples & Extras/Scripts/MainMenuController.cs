using UnityEngine;
using UnityEngine.SceneManagement; // Essential for switching scenes

public class MainMenuController : MonoBehaviour
{
    // Method called by the "Play" button
    public void PlayGame()
    {
        // Replace "GameScene" with the EXACT name of your game scene
        SceneManager.LoadScene("GameScene");
    }

    // Method called by the "Shop" button
    public void OpenShop()
    {
        // Replace "ShopScene" with the EXACT name of your shop scene
        SceneManager.LoadScene("ShopScene");
    }

    // Method called by the "Quit" button
    public void QuitGame()
    {
        Debug.Log("Saliendo de Lovely Carnage..."); // Chivato en consola
        Application.Quit(); // Cierra el juego final (.exe)

        // Esto hace que el botón también funcione mientras pruebas en Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
