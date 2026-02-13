# Xavii's Pixel Art Mod (XPAM)
XPAM is a paint-style pixel art engine for WorldBox with drag-based drawing tools, marquee selection workflows, symmetry painting, transforms, persistent palettes, and project workflows.

## Controls
- `F9`: toggle XPAM.
- `Esc`: close XPAM.
- `Ctrl+Z`: undo.
- `Ctrl+Y`: redo.
- `Ctrl+S`: export PNG.
- `Ctrl+Shift+S`: save XPAM project.
- `Ctrl+O`: load XPAM project.
- `Ctrl+I`: import PNG from the Imports folder.
- `Ctrl+A`: select all.
- `Ctrl+C`: copy selection.
- `Ctrl+X`: cut selection.
- `Ctrl+V`: paste selection.
- `[` / `]`: decrease/increase brush size.
- Arrow keys: move selection.
- `Shift` + arrow keys: duplicate-move selection.
- `1`: symmetry none.
- `2`: symmetry vertical.
- `3`: symmetry horizontal.
- `4`: symmetry quadrant.
- `B`: pencil.
- `E`: eraser.
- `G`: fill.
- `L`: line.
- `R`: rectangle.
- `F`: filled rectangle.
- `C`: circle.
- `V`: filled circle.
- `I`: color picker.
- `H`: replace color.
- `Q`: selection tool.
- `M`: move tool.
- `Delete`: clear selected area.
- Right click on a pixel: pick color.

## Toolbar
- Tools: Pencil, Eraser, Fill, Line, Rectangle, Filled Rectangle, Circle, Filled Circle, Picker, Replace, Select, Move.
- Shape tools are click-drag-release and preview live before commit.
- Brush sizes: 1, 2, 4, 8.
- Canvas sizes: 16x16, 32x32, 64x64, 96x96, 128x128.
- History/actions: Undo, Redo, Clear, Grid On/Off.
- Transforms: Flip X, Flip Y, Rotate CW, Rotate CCW, Shift Left/Right/Up/Down.
- Shift wrapping toggle: Wrap Off/On.
- Symmetry modes: None, Vertical, Horizontal, Quadrant.
- File pipeline: Import PNG, Export PNG, Save Project, Load Project.
- Selection actions: Copy, Cut, Paste, Delete, Select All.
- Color controls: RGBA sliders, quick palette, and a 16-slot persistent custom palette.
  - Custom palette: left click to apply, right click to save current color.
- Vanilla Presets panel:
  - Scrollable list of vanilla sprite presets.
  - Search box to filter presets.
  - `Load on click` toggle to load preset pixels into the XPAM canvas.
  - `Clone on click` toggle to clone the selected preset into a local editable PNG.
  - `Refresh` button to rebuild the preset catalog.

## File Output
- Exported PNG files:
  - `Mods/Xavii's Pixel Art Mod (XPAM)/Exports/`
- Imported PNG source folder:
  - `Mods/Xavii's Pixel Art Mod (XPAM)/Imports/`
- Saved project files (`.xpam.json`):
  - `Mods/Xavii's Pixel Art Mod (XPAM)/Projects/`
- Persistent custom palette file:
  - `Mods/Xavii's Pixel Art Mod (XPAM)/Palettes/xpam_custom_palette.json`
- Cloned vanilla presets:
  - `Mods/Xavii's Pixel Art Mod (XPAM)/Presets/VanillaClones/`
