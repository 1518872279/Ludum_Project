# Fur Sweeping Mechanic

This component adds a dynamic sweeping effect to the Fur Shader System, allowing the fur to respond to mouse movement. Unlike the brushing or splitting mechanics that require clicking and dragging, the sweeping mechanic activates automatically as the player moves their mouse over the fur object, creating a natural, reactive effect.

## Features

- **Motion-Based Interaction:** Fur reacts to mouse movement without requiring clicks
- **Directional Effect:** Fur is displaced in the direction of the mouse movement
- **Momentum System:** Sweeping effects have natural momentum and gradual fade-out
- **Customizable Settings:** Adjust strength, radius, falloff, and momentum
- **Layer-based Effect:** Movement affects the fur tips more than the base for realism

## Setup Instructions

1. Make sure you have already set up the Fur Shader System on your object
2. Add the `FurSweeping` component to the same GameObject that has the `FurRenderer` component
3. Assign a camera to the "Main Camera" field (if left empty, it will try to find Camera.main)
4. Adjust the sweep settings to your preference:
   - **Sweep Radius:** The area affected around the mouse position
   - **Max Sweep Strength:** The maximum displacement strength possible
   - **Sweep Falloff:** How quickly the effect fades at the edges (higher = sharper falloff)
   - **Sweep Momentum:** How much the effect persists after movement stops (0-1)
   - **Movement Sensitivity:** How responsive the effect is to mouse movement

## How It Works

The sweeping mechanic works by:

1. **Tracking Mouse Movement:** The script continuously monitors the mouse position and calculates the movement delta between frames.

2. **Direction and Strength Calculation:** When the mouse moves, the script determines both the direction and speed of movement, which translate into the direction and strength of the sweeping effect.

3. **Raycasting to Object:** A ray is cast from the camera through the current mouse position to determine where on the fur object the sweep should be applied.

4. **Momentum System:** The sweep effect builds up with continuous movement and gradually fades out when movement stops, creating natural-looking fur behavior.

5. **Vertex Displacement:** The shader displaces fur vertices within the sweep radius in the direction of mouse movement, with displacement strength decreasing with distance from the sweep center.

6. **Layer-based Effect:** The sweep effect is stronger on the outer shell layers (the fur tips) and weaker on the inner layers (the fur base), creating a realistic bending effect.

## Integration with Other Mechanics

The sweeping mechanic can be used alongside other fur interaction mechanics:

- **Compatible with Brushing:** The sweeping effect can coexist with the click-based brushing mechanic, allowing for varied interactions
- **Compatible with Splitting:** Sweeping can be used to create subtle movement before using the splitting mechanic for more precise control
- **Works with Wind:** The wind and sweeping effects combine for complex, natural-looking fur behavior
- **Respects Gravity:** The fur still bends due to gravity while being swept

## Example Uses

1. **Interactive Exhibits:** Create touchless interactive displays where fur reacts to visitor movement
2. **Ambient Motion:** Add subtle ambient motion to fur objects that react to cursor position
3. **Creature Animation:** Make fur on creatures react naturally to player movement without explicit interaction
4. **UI Elements:** Create unique fur-based UI elements that respond to mouse hovering

## Performance Considerations

The sweeping mechanic is designed to be lightweight and efficient:

- Only applies when the mouse moves significantly, reducing unnecessary calculations
- Uses momentum to smooth out the effect and reduce jitter
- The effect gracefully degrades on lower-powered devices by reducing sensitivity
- For very dense fur (high shell count), consider using a smaller sweep radius to maintain performance 