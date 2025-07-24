using UnityEngine;
using TMPro; // Pour TextMeshPro

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private GameObject victoryPanel; // Panel de victoire
    [SerializeField] private TextMeshProUGUI victoryText; // Texte de victoire
    
    [Header("Score Settings")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int starsCollected = 0;
    [SerializeField] private int maxStars = 4; // Nombre d'étoiles pour gagner
    
    private bool gameWon = false; // Pour éviter de gagner plusieurs fois
    
    public static ScoreManager Instance;
    
    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateUI();
        
        // S'assurer que le panel de victoire est caché au début
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }
    
    public void AddScore(int points)
    {
        if (gameWon) return; // Ne plus compter si le jeu est gagné
        
        currentScore += points;
        starsCollected++;
        
        UpdateUI();
        
        // Vérifier si le joueur a gagné
        CheckVictory();
        
        Debug.Log($"Étoiles: {starsCollected}/{maxStars}");
    }
    
    void CheckVictory()
    {
        if (starsCollected >= maxStars && !gameWon)
        {
            gameWon = true;
            ShowVictory();
        }
    }
    
    void ShowVictory()
    {
        Debug.Log("🎉 VICTOIRE ! 🎉");
        
        // Afficher le panel de victoire
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        // Mettre à jour le texte de victoire
        if (victoryText != null)
        {
            victoryText.text = $"🎉 VICTOIRE ! 🎉\nScore final: {currentScore}\nToutes les étoiles collectées !";
        }
        
        // Arrêter le temps du jeu
        Time.timeScale = 0f;
        
        // Déverrouiller le curseur pour cliquer sur les boutons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Étoiles: {starsCollected}/{maxStars}";
        }
    }
    
    // Méthodes publiques pour les boutons de victoire
    public void RestartGame()
    {
        Time.timeScale = 1f; // Remettre le temps normal
        ResetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1f; // Remettre le temps normal
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    
   
    public int GetScore()
    {
        return currentScore;
    }
    
    public int GetStarsCollected()
    {
        return starsCollected;
    }
    
    // Réinitialiser le score
    public void ResetScore()
    {
        currentScore = 0;
        starsCollected = 0;
        gameWon = false;
        UpdateUI();
    }
}
