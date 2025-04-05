using UnityEngine;

[ExecuteInEditMode]
public class FurRenderer : MonoBehaviour
{
    [Range(1, 32)]
    public int shellCount = 16;
    
    [Range(0.0f, 0.2f)]
    public float furLength = 0.05f;
    
    [Range(0.0f, 2.0f)]
    public float furDensity = 1.0f;
    
    [Range(0.0f, 10.0f)]
    public float furThinness = 1.0f;
    
    [Range(0.0f, 1.0f)]
    public float furShading = 0.25f;
    
    public Color furColor = Color.white;
    
    public Vector4 windDirection = new Vector4(1, 0, 0, 0);
    
    [Range(0.0f, 1.0f)]
    public float windStrength = 0.5f;
    
    [Range(0.0f, 1.0f)]
    public float furGravityStrength = 0.25f;
    
    public Texture2D furPattern;
    
    private Material furMaterial;
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;
    
    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("No renderer found on the object. FurRenderer component requires a renderer.");
            enabled = false;
            return;
        }
        
        // Create material instance
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
        
        propertyBlock = new MaterialPropertyBlock();
    }
    
    private void OnDisable()
    {
        // Clean up
        if (furMaterial != null)
        {
            if (Application.isPlaying)
                Destroy(furMaterial);
            else
                DestroyImmediate(furMaterial);
        }
    }
    
    private void Update()
    {
        if (rend == null || furMaterial == null) return;
        
        // Update material properties
        furMaterial.SetTexture("_MainTex", rend.sharedMaterial.GetTexture("_MainTex"));
        furMaterial.SetTexture("_FurTex", furPattern);
        furMaterial.SetFloat("_FurLength", furLength);
        furMaterial.SetFloat("_FurDensity", furDensity);
        furMaterial.SetFloat("_FurThinness", furThinness);
        furMaterial.SetFloat("_FurShading", furShading);
        furMaterial.SetColor("_FurColor", furColor);
        furMaterial.SetVector("_WindDirection", windDirection.normalized);
        furMaterial.SetFloat("_WindStrength", windStrength);
        furMaterial.SetFloat("_FurGravityStrength", furGravityStrength);
        
        // Draw the base mesh with the object's original material
        Graphics.DrawMesh(rend.GetComponent<MeshFilter>().sharedMesh, transform.localToWorldMatrix,
            rend.sharedMaterial, gameObject.layer);
        
        // Draw each fur shell layer
        for (int i = 0; i < shellCount; i++)
        {
            float layerValue = (float)(i + 1) / shellCount;
            
            propertyBlock.Clear();
            propertyBlock.SetFloat("_Layer", layerValue);
            
            Graphics.DrawMesh(rend.GetComponent<MeshFilter>().sharedMesh, transform.localToWorldMatrix,
                furMaterial, gameObject.layer, null, 0, propertyBlock);
        }
    }
} 