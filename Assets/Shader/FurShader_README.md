# Fur Shader System

This package contains a dynamic fur shader system for Unity that creates realistic-looking fur using a shell-based rendering approach. The system includes these main components:

1. **FurShader.shader** - The shader that handles the fur's appearance and behavior
2. **FurRenderer.cs** - A component that renders multiple shell layers to create volumetric fur
3. **FurPatternGenerator.cs** - A utility for generating fur pattern textures
4. **FurBrushing.cs** - A component that adds interactive brushing functionality
5. **FurSplitting.cs** - A component that adds interactive fur splitting functionality
6. **FurSweeping.cs** - A component that adds motion-based fur sweeping functionality

## Setup Instructions

### 1. Generate a Fur Pattern Texture

First, you need to create a fur pattern texture that will define where the fur appears:

1. Add the `FurPatternGenerator` component to any GameObject in your scene
2. Go to the menu: Tools > Generate Fur Pattern
3. Adjust the settings on the FurPatternGenerator component to change the fur pattern appearance
4. A new texture asset called "FurPattern.asset" will be created in the Shader folder

### 2. Apply Fur to an Object

To add fur to any mesh in your scene:

1. Add the `FurRenderer` component to the GameObject with a MeshRenderer
2. Assign the generated fur pattern texture to the "Fur Pattern" field
3. Adjust the following properties to customize the fur appearance:
   - **Shell Count**: Number of layers (higher = denser but more performance intensive)
   - **Fur Length**: How long the fur strands are
   - **Fur Density**: How thick/dense the fur appears
   - **Fur Thinness**: Controls the scale of the fur pattern
   - **Fur Shading**: Controls the shading gradient from root to tip
   - **Fur Color**: Tints the fur color
   - **Wind Direction**: Direction the fur sways
   - **Wind Strength**: Intensity of the wind effect
   - **Fur Gravity Strength**: How much the fur bends downward due to gravity

### 3. Add Interactive Mechanics (Optional)

You can add interactive mechanics to the fur to allow players to interact with it:

#### Brushing Mechanic

To allow players to brush the fur using the mouse:

1. Add the `FurBrushing` component to the same GameObject that has the `FurRenderer` component
2. Assign a camera to the "Main Camera" field (if left empty, it will try to find Camera.main)
3. Adjust the brush settings to achieve your desired interaction:
   - **Brush Radius**: The area affected by the brush
   - **Brush Strength**: How strongly the fur is pushed away
   - **Brush Falloff**: How quickly the brush effect fades at the edges
   - **Brush Recovery Speed**: How quickly the fur returns to its original position

#### Splitting Mechanic

To allow players to split the fur by dragging across it (useful for revealing hidden details):

1. Add the `FurSplitting` component to the same GameObject that has the `FurRenderer` component
2. Assign a camera to the "Main Camera" field (if left empty, it will try to find Camera.main)
3. Adjust the split settings to achieve your desired interaction:
   - **Split Strength**: How strongly the fur is pushed away from the split line
   - **Split Width**: The width of the area affected by the split
   - **Split Falloff**: How quickly the split effect fades at the edges
   - **Split Recovery Speed**: How quickly the fur returns to its original position
   - **Split Highlight Color**: An optional color to highlight the split area

#### Sweeping Mechanic

To make the fur automatically react to mouse movement without requiring clicks:

1. Add the `FurSweeping` component to the same GameObject that has the `FurRenderer` component
2. Assign a camera to the "Main Camera" field (if left empty, it will try to find Camera.main)
3. Adjust the sweep settings to achieve your desired interaction:
   - **Sweep Radius**: The area affected around the mouse position
   - **Max Sweep Strength**: The maximum displacement strength possible
   - **Sweep Falloff**: How quickly the effect fades at the edges
   - **Sweep Momentum**: How much the effect persists after movement stops
   - **Movement Sensitivity**: How responsive the effect is to mouse movement

## How It Works

The fur shader uses a technique called "shell rendering" where multiple offset copies of the mesh are drawn with varying opacity to create the illusion of volumetric fur. The `FurRenderer` component is responsible for drawing each shell layer with different offset values, while the shader itself handles the fur's appearance and animation.

Each shell layer is rendered with:
1. Displacement along the vertex normal direction
2. Varying fur density based on the layer position (thicker at the base, thinner at the tip)
3. Wind and gravity effects that are more pronounced at the tips

### Interactive Mechanics

The brushing mechanic works by calculating the distance from each vertex to the brush position and applying additional displacement away from the brush center. This displacement is stronger on the outer shell layers (the fur tips) and weaker on the inner layers (the fur base), creating a realistic brushing effect.

The splitting mechanic works by calculating the distance from each vertex to a line defined by the start and end points of the player's drag motion. Vertices close to this line are displaced perpendicular to it, creating a parting effect that reveals anything hidden beneath the fur. Like the brushing mechanic, the effect is stronger on the tips and weaker at the base.

The sweeping mechanic works by continuously tracking mouse movement and applying displacement to fur vertices in the direction of that movement. It includes a momentum system that makes the effect build up with continuous movement and gradually fade out when movement stops, creating natural-looking fur behavior. This provides a more ambient, reactive interaction compared to the more deliberate brushing and splitting mechanics.

## Performance Considerations

The performance impact of the fur shader depends primarily on the Shell Count parameter. For mobile or lower-end devices, consider using a lower shell count (8-12) and a simpler fur pattern texture. For high-end devices, you can use higher shell counts (16-32) for more realistic results.

The interactive mechanics add minimal overhead to the fur shader system, as they reuse many of the existing displacement calculations. However, for very dense fur, you may want to reduce the interaction radii/widths or simplify other fur parameters to maintain performance.

## Custom Fur Patterns

For more control, you can create custom fur pattern textures in an external image editor:
- Use grayscale images where white represents fur and black represents gaps
- Import the texture into Unity and assign it to the "Fur Pattern" field on the FurRenderer component
- Make sure to set the texture import settings to:
  - Wrap Mode: Repeat
  - Filter Mode: Bilinear or Trilinear
  - Compression: Use quality compression appropriate for your platform 