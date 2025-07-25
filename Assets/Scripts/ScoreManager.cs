using UnityEngine;
using TMPro; // Pour TextMeshPro

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private GameObject victoryPanel; // Panel de victoire
    [SerializeField] private TextMeshProUGUI victoryText; // Texte de victoire
    [SerializeField] private GameObject defeatPanel; // Panel de défaite
    [SerializeField] private TextMeshProUGUI defeatText; // Texte de défaite
    [SerializeField] private TextMeshProUGUI timerText; // Texte du chrono
    
    [Header("Score Settings")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int starsCollected = 0;
    [SerializeField] private int maxStars = 4; // Nombre d'étoiles pour gagner
    
    [Header("Timer Settings")]
    [SerializeField] private float gameTime = 45f; // 45 secondes
    
    private bool gameWon = false; 
    private bool gameLost = false; 
    private float currentTime; 
    
    public static ScoreManager Instance;
    
    void Awake()
    {
        // Plus de DontDestroyOnLoad - on laisse le ScoreManager se recréer à chaque scène
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Initialiser le chrono
        currentTime = gameTime;
    }
    
    void Start()
    {
        // Plus besoin de RefreshUIReferences - les références sont assignées dans l'inspecteur
        UpdateUI();
        
        // S'assurer que les panels sont cachés au début
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }
    }
    
    void Update()
    {
        // Gérer le chrono seulement si le jeu n'est pas fini
        if (!gameWon && !gameLost)
        {
            currentTime -= Time.deltaTime;
            UpdateUI();
            
            // Vérifier si le temps est écoulé
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                gameLost = true;
                ShowDefeat();
            }
        }
    }
    
    
    
    public void AddScore(int points)
    {
        if (gameWon || gameLost) return; // Ne plus compter si le jeu est fini

        currentScore += points;
        starsCollected++;

        UpdateUI();

        // Vérifier si le joueur a gagné
        CheckVictory();

        Debug.Log($"Étoiles: {starsCollected}/{maxStars}");
    }
    
    void CheckVictory()
    {
        if (starsCollected >= maxStars && !gameWon && !gameLost)
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
            float timeBonus = Mathf.RoundToInt(currentTime);
            victoryText.text = $"🎉 VICTOIRE ! 🎉\nScore final: {currentScore}\nTemps restant: {timeBonus}s\nToutes les étoiles collectées !";
        }
        
        // Arrêter le temps du jeu
        Time.timeScale = 0f;
        
        // Déverrouiller le curseur pour cliquer sur les boutons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void ShowDefeat()
    {
        Debug.Log("💀 DÉFAITE ! 💀");
        
        // Afficher le panel de défaite
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
        
        // Mettre à jour le texte de défaite
        if (defeatText != null)
        {
            defeatText.text = $"💀 TEMPS ÉCOULÉ ! 💀\nÉtoiles collectées: {starsCollected}/{maxStars}\nScore: {currentScore}\nEssayez encore !";
        }
        
        // Arrêter le temps du jeu
        Time.timeScale = 0f;
        
        // Déverrouiller le curseur pour cliquer sur les boutons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void UpdateUI()
    {
        // Mettre à jour le score
        if (scoreText != null)
        {
            scoreText.text = $"Étoiles: {starsCollected}/{maxStars}";
        }
        
        // Mettre à jour le chrono
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"Temps: {minutes:00}:{seconds:00}";
            
            // Changer la couleur si le temps devient critique (moins de 10 secondes)
            if (currentTime <= 10f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }
    
    // Méthodes publiques pour les boutons de victoire
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        ResetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1f; 
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
        gameLost = false;
        currentTime = gameTime; // Réinitialiser le chrono
        
        Time.timeScale = 1f;
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }
        
        UpdateUI();
    }
}
