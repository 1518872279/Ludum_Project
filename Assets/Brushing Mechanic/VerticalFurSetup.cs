using UnityEngine;

[RequireComponent(typeof(FurRenderer))]
[RequireComponent(typeof(FurSweeping))]
public class VerticalFurSetup : MonoBehaviour
{
    [Header("Quick Setup")]
    [Range(0f, 1f)]
    [Tooltip("0 = Use normal direction, 1 = Use vertical direction")]
    public float verticalFurStrength = 1.0f;
    
    [Header("Fur Settings")]
    [Range(0.01f, 0.2f)]
    public float furLength = 0.05f;
    [Range(0.5f, 2.0f)]
    public float furDensity = 1.0f;
    [Range(8, 24)]
    public int shellCount = 16;
    
    [Header("Sweep Settings")]
    public bool optimizeSweepForVerticalFur = true;
    
    private FurRenderer furRenderer;
    private FurSweeping furSweeping;
    
    private void OnEnable()
    {
        furRenderer = GetComponent<FurRenderer>();
        furSweeping = GetComponent<FurSweeping>();
        
        if (furRenderer == null || furSweeping == null)
        {
            Debug.LogError("Required components (FurRenderer or FurSweeping) not found!");
            return;
        }
        
        // Apply initial settings
        ApplySettings();
    }
    
    private void OnValidate()
    {
        if (furRenderer != null && furSweeping != null)
        {
            ApplySettings();
        }
    }
    
    private void ApplySettings()
    {
        // Apply fur settings
        furRenderer.useVerticalDirection = verticalFurStrength;
        furRenderer.verticalDirection = Vector3.up; // You could make this customizable if needed
        furRenderer.furLength = furLength;
        furRenderer.furDensity = furDensity;
        furRenderer.shellCount = shellCount;
        
        // Apply sweep settings for vertical fur
        furSweeping.optimizeForVerticalFur = optimizeSweepForVerticalFur;
        furSweeping.horizontalBias = 0.8f; // Default good value for horizontal bias
        furSweeping.addDownwardBias = true; // Enable downward bias to better reveal surface
        furSweeping.downwardBiasStrength = 0.2f; // Default good value for downward bias
        
        // Increase sweep radius for better coverage with vertical fur
        if (optimizeSweepForVerticalFur)
        {
            furSweeping.sweepRadius = 0.7f;
            furSweeping.maxSweepStrength = 0.3f;
        }
    }
} 