using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Jeu";
    [SerializeField] private string creditsSceneName = "Crédits";

    [SerializeField] private string menuSceneName = "Menu";


    public void PlayGame()
    {
        Debug.Log("Chargement de la scène Jeu");
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMenu()
    {
        Debug.Log("Retour au menu principal");
        SceneManager.LoadScene(menuSceneName);
    }
    
    
    public void OpenCredits()
    {
        Debug.Log("Chargement de la scène Crédits");
        SceneManager.LoadScene(creditsSceneName);
    }
    
    
    public void QuitGame()
    {
        Debug.Log("Quitter l'application");
        Application.Quit();
        
        // Pour tester dans l'éditeur Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
