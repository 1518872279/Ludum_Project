# Fur Sweeping Mechanic with Vertical Fur Orientation

This document explains how to modify a dynamic fur system so that the fur is oriented vertically (e.g., always pointing upward) rather than following the object's surface. This vertical orientation makes it easier for the sweeping mechanic—driven by continuous mouse movement—to reveal the object's surface details.

## Key Concepts

- **Vertical Fur Orientation:**  
  Instead of using the vertex normal for fur displacement, we use a fixed vertical direction (e.g., `(0, 1, 0)`). This causes the fur to stand upright regardless of the object's curvature, improving surface visibility.

- **Sweeping Mechanic:**  
  The sweeping effect is calculated based on the player's mouse movement. Every frame, a raycast determines the mouse's hit point on the fur object. The script calculates a sweep direction and strength from the mouse movement delta, then passes these parameters to the fur shader to displace vertices within a specified radius.

## Modified Vertex Shader Snippet

Below is an example snippet for the vertex shader. The shader now uses a fixed vertical direction for base fur displacement and adds a sweeping effect based on input parameters:

```c
// Shader Properties
_VerticalDir ("Vertical Direction", Vector) = (0,1,0,0)
_FurLength ("Fur Length", Range(0,0.2)) = 0.05
_SweepPos ("Sweep Position", Vector) = (0,0,0,0)
_SweepDir ("Sweep Direction", Vector) = (0,0,0,0)
_SweepRadius ("Sweep Radius", Float) = 0.5
_SweepStrength ("Sweep Strength", Float) = 0

// Vertex shader snippet
v2f vert (appdata v)
{
    v2f o;

    // Use fixed vertical direction for fur displacement
    float3 verticalDir = _VerticalDir.xyz;  // Typically (0,1,0)
    float3 offset = verticalDir * _FurLength;

    // Apply sweep effect if the vertex is within the sweep radius
    // (Assume v.vertex is in local space)
    float distanceToSweep = distance(v.vertex.xyz, _SweepPos.xyz);
    if (distanceToSweep < _SweepRadius)
    {
        // Influence diminishes with distance from the sweep center
        float influence = (_SweepRadius - distanceToSweep) / _SweepRadius;
        offset += _SweepDir.xyz * _SweepStrength * influence;
    }

    // Calculate the final vertex position with displacement
    float4 displacedVertex = v.vertex + float4(offset, 0);
    o.vertex = UnityObjectToClipPos(displacedVertex);
    o.uv = v.uv;
    return o;
}
