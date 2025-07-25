using UnityEngine;

public class Star : MonoBehaviour
{
    [Header("Star Settings")]
    [SerializeField] private int pointValue = 1; 
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private AudioClip collectSound; 
    
    private AudioSource audioSource;
    
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(pointValue);
            
         
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
            
            
            CollectEffect();
            
        
            Destroy(gameObject);
        }
    }
    
    void CollectEffect()
    {
       
        transform.localScale *= 1.2f;
    }
}
