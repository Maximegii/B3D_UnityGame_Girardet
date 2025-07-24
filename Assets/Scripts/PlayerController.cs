using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpHeight = 8f; // Hauteur du saut
    [SerializeField] private float gravity = 20f; // Force de gravité
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform; // Référence à la caméra
    
    private CharacterController characterController;
    private Vector3 moveDirection;
    private bool isMoving = false;
    private float verticalVelocity = 0f; // Vitesse verticale pour le saut
    private bool isGrounded = true; // Notre propre détection du sol
    
    void Start()
    {
        // Récupérer les composants
        characterController = GetComponent<CharacterController>();
        
        // Si l'animator n'est pas assigné, le chercher automatiquement
        if (animator == null)
            animator = GetComponent<Animator>();
            
        // Si la caméra n'est pas assignée, chercher la caméra principale
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }
    
    void Update()
    {
        HandleMovementInput();
        HandleJumpInput(); // Ajouter la gestion du saut
        MovePlayer();
        UpdateAnimations();
    }
    
    void HandleMovementInput()
    {
        
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");     
        
        // Calculer la direction de mouvement relative à la caméra
        if (cameraTransform != null)
        {
            // Obtenir les directions avant et droite de la caméra (sans l'inclinaison verticale)
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;
            
            // Projeter sur le plan horizontal (éliminer le Y)
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            
            // Normaliser les vecteurs
            cameraForward.Normalize();
            cameraRight.Normalize();
            
            // Calculer la direction de mouvement
            moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        }
        else
        {
           
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        }
        
        
        isMoving = moveDirection.magnitude > 0.1f;
    }
    
    void HandleJumpInput()
    {
        // Améliorer la détection du sol
        isGrounded = characterController.isGrounded;
        
        // Si on tombe et qu'on touche le sol, réinitialiser immédiatement
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Petite valeur négative pour rester "collé" au sol
        }
        
        // Saut uniquement si on est au sol
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = jumpHeight;
            isGrounded = false; // Empêcher le saut multiple immédiat
        }
        
        // Appliquer la gravité si on n'est pas au sol
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }
    
    void MovePlayer()
    {
        Vector3 movement = Vector3.zero;
        
        // Déplacement horizontal (que le personnage bouge ou non)
        if (isMoving)
        {
            movement = moveDirection * moveSpeed * Time.deltaTime;
            
            // Faire tourner le personnage dans la direction du mouvement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Ajouter le mouvement vertical (saut/gravité) - toujours appliqué
        movement.y = verticalVelocity * Time.deltaTime;
        
        // Appliquer le mouvement
        characterController.Move(movement);
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            // Animation de marche
            animator.SetBool("IsWalking", isMoving);
            
            
            
            // Vitesse
            float speed = isMoving ? 5f : 0f; 
            animator.SetFloat("Speed", speed);
        }
        else
        {
            Debug.LogWarning("Animator non assigné !");
        }
    }
}
