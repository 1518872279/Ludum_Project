# Fur Splitting Mechanic for Dynamic Fur System

This document explains the implementation of a fur splitting mechanic in Unity. Instead of brushing away the fur, the player will perform a split gesture—clicking and dragging across the fur—to create a visible separation along a split line. This effect can be used to reveal hidden lice or other details beneath the fur.

## Concept Overview

- **Mouse Input & Drag Detection:**  
  The script captures the player's mouse input. When the player clicks and drags over the fur, the start and end points of the drag are recorded (converted into local space). These two points define a line along which the fur will be split.

- **Shader Interaction:**  
  The script sends the split parameters—the start and end positions of the drag and a configurable split strength—to the fur shader. The shader uses these values to apply vertex displacement near the split line, simulating a separation in the fur.

- **Visual Effect:**  
  The splitting effect displaces vertices near the split line. Typically, vertices on either side of the line are pushed away in opposite directions, creating a gap that visually represents a split.

## C# Script: FurSplitting.cs

Below is the complete C# script that implements the fur splitting mechanic:

```csharp
using UnityEngine;

public class FurSplitting : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               // Reference to the main camera
    public Material furMaterial;            // The material using the fur shader

    [Header("Split Settings")]
    public float splitStrength = 0.2f;      // Strength of the split displacement

    private bool isSplitting = false;
    private Vector3 splitStart;
    private Vector3 splitEnd;

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
                furMaterial.SetVector("_SplitStart", splitStart);
                furMaterial.SetVector("_SplitEnd", splitEnd);
                furMaterial.SetFloat("_SplitStrength", splitStrength);
            }
        }

        // End splitting on mouse button release
        if (Input.GetMouseButtonUp(0))
        {
            isSplitting = false;
            // Reset the split strength to remove the effect
            furMaterial.SetFloat("_SplitStrength", 0);
        }
    }
}
