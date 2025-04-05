# Brushing Mechanic for Dynamic Fur System

This document explains the brushing mechanic for a dynamic fur system in Unity. In our game, players interact with the fur by using their mouse to "brush" the fur away. The mechanic works as follows:

- **Mouse Input & Raycasting:**  
  The script detects when the player presses the left mouse button. It then casts a ray from the camera through the mouse position. If the ray hits the fur object, the hit point is calculated in local space.

- **Shader Interaction:**  
  The local hit point, along with configurable brush parameters (radius and strength), is sent to the fur shader. The shader then applies additional displacement to vertices within the brush area, simulating a brushing effect that moves the fur aside.

- **Resetting the Brush:**  
  When the player stops brushing (releases the mouse button), the brush effect resets so that the fur returns to its normal state.

## C# Script: FurBrushing.cs

Below is the complete C# script that implements this mechanic:

```csharp
using UnityEngine;

public class FurBrushing : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               // Reference to the main camera
    public Material furMaterial;            // The material using the fur shader

    [Header("Brush Settings")]
    public float brushRadius = 0.5f;        // Radius of the brush effect
    public float brushStrength = 0.1f;      // Strength of the brush displacement

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
                    furMaterial.SetVector("_BrushPos", localHitPoint);
                    furMaterial.SetFloat("_BrushRadius", brushRadius);
                    furMaterial.SetFloat("_BrushStrength", brushStrength);
                }
            }
        }
        else
        {
            // Reset the brush effect when the mouse button is not pressed
            furMaterial.SetFloat("_BrushStrength", 0);
        }
    }
}
