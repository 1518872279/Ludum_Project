# Fur Splitting Mechanic

This component adds an interactive splitting feature to the Fur Shader System, allowing players to create a parting in the fur by dragging their mouse across it. This effect is useful for revealing hidden details beneath the fur, such as parasites, patterns, or different colored skin.

## Features

- **Line-based Splitting:** Drag across the fur to create a parting along a defined line
- **Customizable Split Settings:** Adjust split width, strength, falloff, and recovery speed
- **Visual Highlighting:** Optional highlight effect along the split line
- **Smooth Recovery:** Fur gradually returns to its original position after splitting
- **Layer-based Effect:** Split affects the fur tips more than the base for realistic movement

## Setup Instructions

1. Make sure you have already set up the Fur Shader System on your object
2. Add the `FurSplitting` component to the same GameObject that has the `FurRenderer` component
3. Assign a camera to the "Main Camera" field (if left empty, it will try to find Camera.main)
4. Adjust the split settings to your preference:
   - **Split Strength:** How strongly the fur is pushed away from the split line
   - **Split Width:** The width of the area affected by the split
   - **Split Falloff:** How quickly the split effect fades at the edges (higher = sharper falloff)
   - **Split Recovery Speed:** How quickly the fur returns to its original position after splitting
   - **Split Highlight Color:** An optional color to highlight the split area

## How It Works

The splitting mechanic works by:

1. **Detecting Drag Motion:** When the player clicks and drags across the fur, the system captures the start and end points of the drag as a line.

2. **Calculating Split Line:** These two points define a line segment in local space that determines where the fur will be parted.

3. **Displacing Fur Vertices:** The shader calculates the distance from each vertex to the split line. If a vertex is within the split width, it's displaced away from the line.

4. **Direction Calculation:** The displacement direction is perpendicular to the line, pushing the fur away on both sides to create a realistic parting effect.

5. **Layer-based Displacement:** The displacement is stronger on the outer shell layers (the fur tips) and weaker on the inner layers (the fur base), creating a realistic parting effect.

6. **Visual Highlighting:** An optional color highlight can be applied along the split line to emphasize the effect or indicate interactive elements beneath.

7. **Gradual Recovery:** When the player stops interacting, the fur gradually returns to its original position based on the recovery speed setting.

## Integration with Other Systems

The splitting mechanic is designed to work alongside the existing fur features:

- **Compatible with Brushing:** The splitting effect can be used together with the brushing mechanic for complex interactions
- **Wind Effects:** Wind and splitting effects are applied simultaneously, adding natural movement to parted fur
- **Gravity:** The fur still bends due to gravity even when split
- **Fur Patterns:** The splitting effect respects the density patterns defined by the fur texture

## Example Use Cases

1. **Grooming Simulator:** Create a virtual pet grooming experience where players split the fur to find and remove ticks or parasites
2. **Hidden Clues:** Hide symbols or patterns underneath the fur that need to be discovered
3. **Wound Inspection:** In a veterinary or medical game, reveal injuries that need treatment
4. **Interactive Storytelling:** Use the splitting mechanic to create moments of discovery or reveal story elements

## Performance Considerations

The splitting effect adds minimal overhead to the fur shader system, as it reuses many of the existing displacement calculations. However, for very dense fur (high shell count), you may want to reduce the split width or simplify other fur parameters to maintain performance. 