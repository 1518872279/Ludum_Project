using UnityEngine;

public class FurSplitting : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               // Reference to the main camera
    
    [Header("Split Settings")]
    public float splitStrength = 0.2f;      // Strength of the split displacement
    public float splitWidth = 0.1f;         // Width of the split
    public float splitFalloff = 1.5f;       // Falloff of the split effect (higher = sharper falloff)
    public float splitRecoverySpeed = 3.0f; // How quickly the fur recovers after splitting
    public Color splitHighlightColor = new Color(1.0f, 1.0f, 1.0f, 0.3f); // Optional highlight color along the split

    private FurRenderer furRenderer;
    private bool isSplitting = false;
    private Vector3 splitStart;
    private Vector3 splitEnd;
    private float splitRecoveryTimer = 0;

    void Start()
    {
        // Try to get the camera if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Get the FurRenderer component
        furRenderer = GetComponent<FurRenderer>();
        if (furRenderer == null)
        {
            Debug.LogError("FurSplitting requires a FurRenderer component on the same GameObject.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Begin splitting on mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits the fur object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                isSplitting = true;
                // Record the starting point of the split in local space
                splitStart = transform.InverseTransformPoint(hit.point);
                
                // Initialize the end point to the same as start
                splitEnd = splitStart;
                
                // Reset recovery timer
                splitRecoveryTimer = 0;
            }
        }

        // Update the split endpoint while the mouse button is held down
        if (isSplitting && Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Record the current endpoint in local space
                splitEnd = transform.InverseTransformPoint(hit.point);

                // Send split parameters to the shader
                Shader.SetGlobalVector("_SplitStart", splitStart);
                Shader.SetGlobalVector("_SplitEnd", splitEnd);
                Shader.SetGlobalFloat("_SplitStrength", splitStrength);
                Shader.SetGlobalFloat("_SplitWidth", splitWidth);
                Shader.SetGlobalFloat("_SplitFalloff", splitFalloff);
                Shader.SetGlobalColor("_SplitHighlightColor", splitHighlightColor);
            }
        }

        // End splitting on mouse button release
        if (Input.GetMouseButtonUp(0) && isSplitting)
        {
            // Don't immediately remove the effect - start recovery
            splitRecoveryTimer = 0;
        }
        
        // Handle split recovery
        if (isSplitting && !Input.GetMouseButton(0))
        {
            splitRecoveryTimer += Time.deltaTime;
            float recoveryFactor = Mathf.Clamp01(splitRecoveryTimer / splitRecoverySpeed);
            
            // Gradually reduce split strength based on the recovery factor
            float currentStrength = Mathf.Lerp(splitStrength, 0, recoveryFactor);
            Shader.SetGlobalFloat("_SplitStrength", currentStrength);
            
            // Once fully recovered, reset the splitting state
            if (recoveryFactor >= 1.0f)
            {
                Shader.SetGlobalFloat("_SplitStrength", 0);
                isSplitting = false;
            }
        }
    }

    void OnDisable()
    {
        // Reset split effects when disabled
        Shader.SetGlobalFloat("_SplitStrength", 0);
    }
} 