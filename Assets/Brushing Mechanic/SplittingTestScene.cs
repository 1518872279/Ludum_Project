using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This script helps set up a test scene for the fur splitting mechanic.
/// It creates a simple object with fur and adds the splitting component.
/// </summary>
public class SplittingTestScene : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Setup Fur Splitting Test")]
    static void SetupFurSplittingTest()
    {
        // Create a new game object with a primitive
        GameObject furObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        furObject.name = "FurSplitObject";
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
        
        // Add the FurSplitting component
        FurSplitting furSplitting = furObject.AddComponent<FurSplitting>();
        furSplitting.mainCamera = Camera.main;
        
        // Set up scene camera if needed
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            Camera camera = cameraObj.AddComponent<Camera>();
            cameraObj.transform.position = new Vector3(0, 1, -3);
            cameraObj.transform.LookAt(furObject.transform);
            
            furSplitting.mainCamera = camera;
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
        
        // Create a "hidden object" under the fur to demonstrate the split revealing something
        GameObject hiddenObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hiddenObject.name = "HiddenObject";
        hiddenObject.transform.SetParent(furObject.transform);
        hiddenObject.transform.localPosition = Vector3.zero;
        hiddenObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f); // Slightly smaller than parent
        
        // Set a different material for the hidden object
        Material hiddenMaterial = new Material(Shader.Find("Standard"));
        hiddenMaterial.color = new Color(1f, 0.2f, 0.2f); // Reddish color
        hiddenObject.GetComponent<Renderer>().material = hiddenMaterial;
        
        // Select the fur object
        Selection.activeGameObject = furObject;
        
        Debug.Log("Fur splitting test scene has been set up. Click and drag across the fur object to split it and reveal the hidden object.");
    }
#endif
} 