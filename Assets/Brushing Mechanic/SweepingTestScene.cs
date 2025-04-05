using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This script helps set up a test scene for the fur sweeping mechanic.
/// It creates a simple object with fur and adds the sweeping component.
/// </summary>
public class SweepingTestScene : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Setup Fur Sweeping Test")]
    static void SetupFurSweepingTest()
    {
        // Create a new game object with a primitive
        GameObject furObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        furObject.name = "FurSweepObject";
        furObject.transform.position = Vector3.zero;
        
        // Add the FurRenderer component
        FurRenderer furRenderer = furObject.AddComponent<FurRenderer>();
        
        // Find or create a fur pattern
        Texture2D furPattern = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Shader/FurPattern.asset");
        if (furPattern == null)
        {
            Debug.LogWarning("No fur pattern found. Please generate one using the FurPatternGenerator first.");
        }
        else
        {
            furRenderer.furPattern = furPattern;
        }
        
        // Add the FurSweeping component
        FurSweeping furSweeping = furObject.AddComponent<FurSweeping>();
        furSweeping.mainCamera = Camera.main;
        
        // Adjust fur renderer properties for better sweep visibility
        furRenderer.furLength = 0.05f;
        furRenderer.furDensity = 1.5f;
        furRenderer.shellCount = 24; // More shells for smoother fur
        
        // Set up scene camera if needed
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            Camera camera = cameraObj.AddComponent<Camera>();
            cameraObj.transform.position = new Vector3(0, 1, -3);
            cameraObj.transform.LookAt(furObject.transform);
            
            furSweeping.mainCamera = camera;
        }
        
        // Add some light
        if (GameObject.FindObjectOfType<Light>() == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.0f;
            lightObj.transform.eulerAngles = new Vector3(50, -30, 0);
        }
        
        // Select the fur object
        Selection.activeGameObject = furObject;
        
        Debug.Log("Fur sweeping test scene has been set up. Move your mouse across the fur object to create sweeping effects.");
    }
#endif
} 