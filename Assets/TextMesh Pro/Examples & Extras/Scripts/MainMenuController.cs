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
        Application.Quit();
        Debug.Log("The game has closed"); // Only visible in Unity Editor
    }
}
