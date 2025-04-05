using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FurPatternGenerator : MonoBehaviour
{
    public int textureSize = 512;
    public float noiseScale = 10f;
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public int seed = 42;
    
    public Texture2D GenerateFurPattern()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Trilinear;
        texture.wrapMode = TextureWrapMode.Repeat;
        
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        
        Color[] colorMap = new Color[textureSize * textureSize];
        
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x / (float)textureSize * noiseScale * frequency) + octaveOffsets[i].x;
                    float sampleY = (y / (float)textureSize * noiseScale * frequency) + octaveOffsets[i].y;
                    
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                
                noiseHeight = Mathf.Clamp01(noiseHeight);
                colorMap[y * textureSize + x] = new Color(noiseHeight, noiseHeight, noiseHeight, 1);
            }
        }
        
        texture.SetPixels(colorMap);
        texture.Apply();
        
        return texture;
    }
    
#if UNITY_EDITOR
    [MenuItem("Tools/Generate Fur Pattern")]
    static void CreateFurPattern()
    {
        FurPatternGenerator generator = FindObjectOfType<FurPatternGenerator>();
        if (generator == null)
        {
            Debug.LogError("Please add a FurPatternGenerator component to a GameObject in the scene.");
            return;
        }
        
        Texture2D furPattern = generator.GenerateFurPattern();
        
        // Save the texture as an asset
        string path = "Assets/Shader/FurPattern.asset";
        AssetDatabase.CreateAsset(furPattern, path);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Fur pattern generated and saved at: " + path);
        
        // Select the texture in the project browser
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = furPattern;
    }
#endif
} 