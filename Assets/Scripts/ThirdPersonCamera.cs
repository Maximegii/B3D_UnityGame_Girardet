using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // Le joueur à suivre
    
    [Header("Camera Settings")]
    [SerializeField] private float distance = 5f; // Distance derrière le joueur
    [SerializeField] private float height = 2f; // Hauteur au-dessus du joueur
    [SerializeField] private float mouseSensitivity = 2f; // Sensibilité de la souris
    [SerializeField] private float smoothSpeed = 10f; // Vitesse de suivi
    
    [Header("Mouse Control")]
    [SerializeField] private float minVerticalAngle = -40f; // Limite rotation vers le bas
    [SerializeField] private float maxVerticalAngle = 80f; // Limite rotation vers le haut
    
    private float horizontalAngle = 0f; // Rotation horizontale (gauche/droite)
    private float verticalAngle = 20f; // Rotation verticale (haut/bas)
    
    void Start()
    {
        // Verrouiller le curseur au centre de l'écran
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        HandleMouseInput();
        UpdateCameraPosition();
    }
    
    void HandleMouseInput()
    {
        // Récupérer le mouvement de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Mettre à jour les angles
        horizontalAngle += mouseX;
        verticalAngle -= mouseY; // Inverser pour un contrôle naturel
        
        // Limiter l'angle vertical
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }
    
    void UpdateCameraPosition()
    {
        // Calculer la rotation de la caméra
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        
        // Calculer la position désirée de la caméra
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        Vector3 desiredPosition = target.position + offset;
        
        // Déplacer la caméra vers la position désirée
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Faire regarder la caméra vers la cible
        transform.LookAt(target.position + Vector3.up * height);
    }
    
    void Update()
    {
        // Déverrouiller le curseur avec Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
