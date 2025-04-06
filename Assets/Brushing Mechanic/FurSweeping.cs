using UnityEngine;

public class FurSweeping : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               // Reference to the main camera
    
    [Header("Sweep Settings")]
    public float sweepRadius = 0.5f;        // Effective radius of the sweep effect
    public float maxSweepStrength = 0.2f;   // Maximum displacement strength for the sweep
    public float sweepFalloff = 1.5f;       // Falloff of the sweep effect (higher = sharper falloff)
    public float sweepMomentum = 0.7f;      // How much the sweep effect persists (0-1)
    public float movementSensitivity = 0.001f; // How sensitive the effect is to mouse movement
    
    [Header("Vertical Fur Settings")]
    [Tooltip("Keeps sweep direction more horizontal for vertical fur")]
    public bool optimizeForVerticalFur = false;
    [Range(0f, 1f)]
    [Tooltip("How much to flatten the sweep direction to be horizontal")]
    public float horizontalBias = 0.8f;
    [Tooltip("Add a slight downward bias to sweep direction to reveal surface")]
    public bool addDownwardBias = false;
    [Range(0f, 0.5f)]
    public float downwardBiasStrength = 0.2f;
    
    private Vector3 lastMousePosition;
    private Vector3 sweepPosition;
    private Vector3 sweepDirection;
    private float currentSweepStrength = 0f;
    private bool isOverFur = false;

    void Start()
    {
        // Try to get the camera if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Initialize the last mouse position
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;
        
        // Only apply sweep effect if the mouse has moved enough
        if (mouseDelta.magnitude > 1.0f)
        {
            // Calculate sweep strength based on the magnitude of mouse movement
            float targetSweepStrength = Mathf.Clamp(mouseDelta.magnitude * movementSensitivity, 0, maxSweepStrength);
            
            // Apply momentum to sweep strength for smoother transitions
            currentSweepStrength = Mathf.Lerp(currentSweepStrength, targetSweepStrength, Time.deltaTime * 10f);
            
            // Raycast from the camera using the current mouse position
            Ray ray = mainCamera.ScreenPointToRay(currentMousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isOverFur = true;
                    
                    // Convert hit point to local space
                    sweepPosition = transform.InverseTransformPoint(hit.point);
                    
                    // Get world space movement direction and convert to local space
                    Vector3 worldMoveDir = new Vector3(mouseDelta.x, mouseDelta.y, 0).normalized;
                    worldMoveDir = mainCamera.transform.TransformDirection(worldMoveDir);
                    sweepDirection = transform.InverseTransformDirection(worldMoveDir);
                    
                    // Optimize for vertical fur if enabled
                    if (optimizeForVerticalFur)
                    {
                        // Project the sweep direction more onto horizontal plane
                        Vector3 horizontalDir = new Vector3(sweepDirection.x, 0, sweepDirection.z).normalized;
                        sweepDirection = Vector3.Lerp(sweepDirection, horizontalDir, horizontalBias);
                        
                        // Add a slight downward component to help reveal the surface
                        if (addDownwardBias)
                        {
                            Vector3 downwardDir = new Vector3(0, -1, 0);
                            sweepDirection = Vector3.Lerp(sweepDirection, downwardDir, downwardBiasStrength);
                        }
                    }
                    
                    // Ensure direction is normalized
                    sweepDirection = sweepDirection.normalized;
                    
                    // Send the sweep parameters to the shader
                    Shader.SetGlobalVector("_SweepPos", sweepPosition);
                    Shader.SetGlobalVector("_SweepDir", sweepDirection);
                    Shader.SetGlobalFloat("_SweepRadius", sweepRadius);
                    Shader.SetGlobalFloat("_SweepStrength", currentSweepStrength);
                    Shader.SetGlobalFloat("_SweepFalloff", sweepFalloff);
                }
                else
                {
                    isOverFur = false;
                }
            }
            else
            {
                isOverFur = false;
            }
        }
        
        // Apply decay to sweep strength when not over fur or when mouse isn't moving much
        if (!isOverFur || mouseDelta.magnitude <= 1.0f)
        {
            currentSweepStrength *= sweepMomentum;
            
            // If sweep strength is very small, reset it to zero
            if (currentSweepStrength < 0.001f)
            {
                currentSweepStrength = 0f;
                Shader.SetGlobalFloat("_SweepStrength", 0f);
            }
            else
            {
                // Update the sweep strength in the shader with the decayed value
                Shader.SetGlobalFloat("_SweepStrength", currentSweepStrength);
            }
        }
        
        // Update last mouse position for the next frame
        lastMousePosition = currentMousePosition;
    }
    
    void OnDisable()
    {
        // Reset sweep effect when disabled
        Shader.SetGlobalFloat("_SweepStrength", 0f);
    }
} 