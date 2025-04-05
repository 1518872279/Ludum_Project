using UnityEngine;

public class FurBrushing : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               // Reference to the main camera
    
    [Header("Brush Settings")]
    public float brushRadius = 0.5f;        // Radius of the brush effect
    public float brushStrength = 0.1f;      // Strength of the brush displacement
    public float brushFalloff = 1.0f;       // Falloff of the brush effect (higher = sharper falloff)
    public float brushRecoverySpeed = 2.0f; // How quickly the fur recovers after brushing

    private FurRenderer furRenderer;
    private Material furMaterial;
    private bool isBrushing = false;
    private Vector3 lastBrushPosition;
    private float brushRecoveryTimer = 0;

    void Start()
    {
        // Try to get the camera if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Get the FurRenderer component
        furRenderer = GetComponent<FurRenderer>();
        if (furRenderer == null)
        {
            Debug.LogError("FurBrushing requires a FurRenderer component on the same GameObject.");
            enabled = false;
            return;
        }

        // Get or create the fur material
        if (furMaterial == null)
        {
            Shader furShader = Shader.Find("Custom/FurShader");
            if (furShader == null)
            {
                Debug.LogError("FurShader not found. Make sure the shader is in your project.");
                enabled = false;
                return;
            }
            
            furMaterial = new Material(furShader);
        }
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            // Cast a ray from the camera to the current mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                // Ensure the hit object is the fur object
                if (hit.collider.gameObject == gameObject)
                {
                    // Convert the hit point from world space to local space
                    Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);

                    // Pass the brush parameters to the shader
                    Shader.SetGlobalVector("_BrushPos", localHitPoint);
                    Shader.SetGlobalFloat("_BrushRadius", brushRadius);
                    Shader.SetGlobalFloat("_BrushStrength", brushStrength);
                    Shader.SetGlobalFloat("_BrushFalloff", brushFalloff);

                    isBrushing = true;
                    lastBrushPosition = localHitPoint;
                    brushRecoveryTimer = 0;
                }
            }
        }
        else if (isBrushing)
        {
            // Handle brush recovery when the mouse button is released
            brushRecoveryTimer += Time.deltaTime;
            float recoveryFactor = Mathf.Clamp01(brushRecoveryTimer / brushRecoverySpeed);
            
            // Gradually reduce brush strength based on the recovery factor
            float currentStrength = Mathf.Lerp(brushStrength, 0, recoveryFactor);
            Shader.SetGlobalFloat("_BrushStrength", currentStrength);
            
            // Once fully recovered, reset the brushing state
            if (recoveryFactor >= 1.0f)
            {
                Shader.SetGlobalFloat("_BrushStrength", 0);
                isBrushing = false;
            }
        }
    }

    void OnDisable()
    {
        // Reset brush effects when disabled
        Shader.SetGlobalFloat("_BrushStrength", 0);
    }
} 