# Fur Brushing Mechanic

This component adds an interactive brushing feature to the Fur Shader System, allowing players to brush and interact with the fur in real-time using the mouse.

## Features

- **Real-time Interaction:** Brush the fur in any direction using the mouse
- **Customizable Brush Settings:** Adjust radius, strength, falloff, and recovery speed
- **Smooth Fur Recovery:** Fur gradually returns to its original position after brushing
- **Layer-based Effect:** Brush affects the fur tips more than the base for realistic movement

## Setup Instructions

1. Make sure you have already set up the Fur Shader System on your object
2. Add the `FurBrushing` component to the same GameObject that has the `FurRenderer` component
3. Assign a camera to the "Main Camera" field (if left empty, it will try to find Camera.main)
4. Adjust the brush settings to your preference:
   - **Brush Radius:** The area affected by the brush
   - **Brush Strength:** How strongly the fur is pushed away
   - **Brush Falloff:** How quickly the brush effect fades at the edges (higher = sharper falloff)
   - **Brush Recovery Speed:** How quickly the fur returns to its original position after brushing

## How It Works

The brushing mechanic works by:

1. **Detecting Mouse Input:** When the player clicks and holds the left mouse button, the system casts a ray from the camera through the cursor position to detect if it hits the fur object.

2. **Calculating Brush Position:** If the ray hits the fur object, it converts the hit point to local space and sends this position to the shader.

3. **Displacing Fur Vertices:** The shader calculates the distance from each vertex to the brush position. If a vertex is within the brush radius, it's displaced away from the brush center.

4. **Layer-based Displacement:** The displacement is stronger on the outer shell layers (the fur tips) and weaker on the inner layers (the fur base), creating a realistic brushing effect.

5. **Gradual Recovery:** When the player stops brushing, the fur gradually returns to its original position based on the recovery speed setting.

## Integration with Other Systems

The brushing mechanic is designed to work alongside the existing fur features:

- **Wind Effects:** Brushing and wind effects are applied simultaneously, allowing for complex fur behavior
- **Gravity:** The fur still bends due to gravity even when being brushed
- **Fur Patterns:** The brushing effect respects the density patterns defined by the fur texture

## Performance Considerations

The brushing effect adds minimal overhead to the fur shader system, as it reuses many of the existing displacement calculations. However, for very dense fur (high shell count), you may want to reduce the brush radius or simplify other fur parameters to maintain performance. 