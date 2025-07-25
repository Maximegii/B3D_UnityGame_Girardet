using UnityEngine;

public class MenuIdleAnimation : MonoBehaviour 
{
    private Animator animator;
    
    void Start() 
    {
        animator = GetComponent<Animator>();
        
        if (animator != null)
        {
            // Forcer l'animation idle en boucle pour le menu
            animator.Play("Idle", 0, 0f);
            
            // Optionnel : désactiver les autres paramètres
            animator.SetBool("IsWalking", false);
            animator.SetFloat("Speed", 0f);
        }
    }
    
    void Update()
    {
        // S'assurer que le personnage reste en idle
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("Speed", 0f);
        }
    }
}
