# Vertical Fur System

This document provides instructions for setting up and using the vertical fur system with an optimized sweeping mechanic. The vertical fur system allows the fur to stand upright (in a consistent direction) rather than following the surface normals, making it easier to reveal the underlying surface with the sweeping mechanic.

## Quick Setup

1. Add a `FurRenderer` component to your GameObject
2. Add a `FurSweeping` component to the same GameObject
3. Add the `VerticalFurSetup` component to the same GameObject
4. Assign a fur pattern texture to the `FurRenderer` component
5. Set up the camera reference in the `FurSweeping` component
6. Adjust the values in the `VerticalFurSetup` component to your liking

## Manual Setup

If you prefer to set up the system manually:

1. Add a `FurRenderer` component to your GameObject
2. In the FurRenderer component:
   - Set `Use Vertical Direction` to a value between 0-1 (0 = use normals, 1 = fully vertical)
   - Ensure the `Vertical Direction` is set to (0, 1, 0) for upward fur
   - Adjust `Fur Length`, `Fur Density`, and other properties as needed
   - Assign your Fur Pattern texture

3. Add a `FurSweeping` component to the same GameObject
4. In the FurSweeping component:
   - Enable `Optimize For Vertical Fur`
   - Set `Horizontal Bias` to around 0.8 for better horizontal sweeping
   - Enable `Add Downward Bias` to help reveal the surface
   - Set `Downward Bias Strength` to around 0.2 for subtle effect
   - Adjust `Sweep Radius` and `Max Sweep Strength` for your model

## How It Works

### Vertical Fur Direction

The vertical fur system works by replacing the standard normal-based fur direction with a consistent vertical direction:

```glsl
// Choose between normal direction and vertical direction
float3 furDirection = lerp(v.normal, normalize(_VerticalDir.xyz), _UseVerticalDir);
```

This creates fur that stands upright regardless of the surface orientation, making it ideal for:
- Ground cover like grass
- Character fur that should maintain a consistent appearance
- Models where normal-based fur would look unnatural

### Optimized Sweeping for Vertical Fur

The standard sweeping mechanic has been enhanced to work better with vertical fur:

1. **Horizontal Projection**:  
   Sweeping directions are projected onto the horizontal plane, making them more effective at parting vertical fur.

2. **Downward Bias**:  
   A subtle downward component can be added to the sweep direction, helping reveal the underlying surface.

3. **Adjusted Parameters**:  
   Sweep radius and strength values are automatically increased when using vertical fur.

## Tips for Best Results

- **Adjust Vertical Strength**: For natural fur, consider using a value less than 1.0 for `verticalFurStrength` to blend between normal and vertical directions
- **Increase Shell Count**: Vertical fur often benefits from more shell layers (16-24) for smoother appearance
- **Tune the Fur Density**: Higher density values work well with vertical fur
- **Experiment with Fur Length**: Vertical fur allows for longer fur without looking unnatural

## Compatibility 

The vertical fur system is fully compatible with the other fur mechanics:
- Fur Brushing
- Fur Splitting

All these mechanics can be used in combination for rich interactive fur effects. 