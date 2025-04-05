# Fur Sweeping Mechanic for Dynamic Fur System

This document outlines how to implement a sweeping mechanic for a dynamic fur system in Unity. Instead of relying on mouse button input, the fur is affected by continuous mouse movement. As the player moves the mouse over the fur object, the script calculates the sweep effect based on the movement's direction and magnitude. This effect is then applied via the fur shader to displace vertices near the mouse's hit point.

## How It Works

- **Mouse Movement Detection:**  
  The script continuously tracks the mouse position and computes the delta (change in position) between frames.

- **Raycasting to the Fur Object:**  
  Every frame, a ray is cast from the camera through the current mouse position. If the ray hits the fur object, the hit point is determined in local space.

- **Sweep Parameters:**  
  - **Sweep Position:** The local hit point on the fur.  
  - **Sweep Direction:** The normalized direction of the mouse movement (converted to local space).  
  - **Sweep Strength:** A value derived from the magnitude of the mouse movement, clamped to a maximum value.  
  - **Sweep Radius:** An area around the hit point where the sweeping effect is applied.

- **Shader Interaction:**  
  The computed parameters are passed to the fur shader. In the shader, vertices within the sweep radius are displaced in the direction of the sweep, with the displacement scaled by the influence (which diminishes with distance from the sweep center).

## C# Script: FurSweeping.cs

Below is the complete C# script that implements the fur sweeping mechanic:

```csharp
using UnityEngine;

public class FurSweeping : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               // Reference to the main camera
    public Material furMaterial;            // Material using the fur shader

    [Header("Sweep Settings")]
    public float sweepRadius = 0.5f;         // Effective radius of the sweep effect
    public float maxSweepStrength = 0.2f;    // Maximum displacement strength for the sweep

    private Vector3 lastMousePosition;

    void Start()
    {
        // Initialize the last mouse position
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;
        
        // Calculate sweep strength based on the magnitude of mouse movement
        float sweepStrength = Mathf.Clamp(mouseDelta.magnitude * 0.001f, 0, maxSweepStrength);

        // Raycast from the camera using the current mouse position
        Ray ray = mainCamera.ScreenPointToRay(currentMousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // Convert hit point to local space
                Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);
                
                // Convert mouse delta to local space direction (approximate)
                Vector3 localSweepDir = transform.InverseTransformDirection(mouseDelta.normalized);

                // Pass the sweep parameters to the fur shader
                furMaterial.SetVector("_SweepPos", localHitPoint);
                furMaterial.SetVector("_SweepDir", localSweepDir);
                furMaterial.SetFloat("_SweepRadius", sweepRadius);
                furMaterial.SetFloat("_SweepStrength", sweepStrength);
            }
            else
            {
                // If the ray doesn't hit the fur object, reset the sweep effect
                furMaterial.SetFloat("_SweepStrength", 0);
            }
        }
        else
        {
            // No hit detected; reset the sweep effect
            furMaterial.SetFloat("_SweepStrength", 0);
        }
        
        // Update lastMousePosition for the next frame
        lastMousePosition = currentMousePosition;
    }
}
