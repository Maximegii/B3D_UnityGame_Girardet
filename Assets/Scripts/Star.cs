using UnityEngine;

public class Star : MonoBehaviour
{
    [Header("Star Settings")]
    [SerializeField] private int pointValue = 1; // Points donnés par cette étoile
    [SerializeField] private float rotationSpeed = 50f; // Vitesse de rotation
    [SerializeField] private AudioClip collectSound; // Son de collecte (optionnel)
    
    private AudioSource audioSource;
    
    void Start()
    {
        // Récupérer l'AudioSource s'il y en a un
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        // Faire tourner l'étoile sur elle-même
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Vérifier si c'est le joueur qui touche l'étoile
        if (other.CompareTag("Player"))
        {
            // Ajouter des points au score
            ScoreManager.Instance.AddScore(pointValue);
            
            // Jouer le son de collecte (si configuré)
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
            
            // Effet visuel de collecte (optionnel)
            CollectEffect();
            
            // Détruire l'étoile
            Destroy(gameObject);
        }
    }
    
    void CollectEffect()
    {
        // Ici vous pouvez ajouter des effets visuels
        // Par exemple : particules, agrandissement, etc.
        
        // Effet simple : agrandir rapidement puis détruire
        transform.localScale *= 1.2f;
    }
}
