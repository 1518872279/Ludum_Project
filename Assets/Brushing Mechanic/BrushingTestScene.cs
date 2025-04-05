using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This script helps set up a test scene for the fur brushing mechanic.
/// It creates a simple object with fur and adds the brushing component.
/// </summary>
public class BrushingTestScene : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Setup Fur Brushing Test")]
    static void SetupFurBrushingTest()
    {
        // Create a new game object with a primitive
        GameObject furObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        furObject.name = "FurObject";
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
        
        // Add the FurBrushing component
        FurBrushing furBrushing = furObject.AddComponent<FurBrushing>();
        furBrushing.mainCamera = Camera.main;
        
        // Set up scene camera if needed
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            Camera camera = cameraObj.AddComponent<Camera>();
            cameraObj.transform.position = new Vector3(0, 1, -3);
            cameraObj.transform.LookAt(furObject.transform);
            
            furBrushing.mainCamera = camera;
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
        
        Debug.Log("Fur brushing test scene has been set up. Click and drag on the fur object to brush it.");
    }
#endif
} 