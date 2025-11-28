# Phase 1: Foundation - Setup Guide

This guide provides step-by-step instructions for setting up a test scene to verify all Phase 1 systems are working correctly.

## Prerequisites

- Unity 6 (6000.1.3f1) installed
- TextMeshPro package imported (Unity will prompt on first use)
- All Phase 1 scripts compiled successfully

## Scene Setup Instructions

### Step 1: Create Test Scene

1. In Unity Editor, go to **File > New Scene**
2. Choose **2D (URP)** template
3. Save the scene as `Assets/Scenes/Phase1Test.scene`

### Step 2: Setup Game Manager

1. Create an empty GameObject: **GameObject > Create Empty**
2. Rename it to `GameManager`
3. Add the **GameManager** component (`Assets/Scripts/Core/GameManager.cs`)
4. The GameManager will automatically create a TimeManager child object

### Step 3: Setup Camera

1. Select the **Main Camera** in the hierarchy
2. Verify it's set to **Orthographic** projection
3. Set **Projection > Size** to `10`
4. Set **Position** to `(0, 0, -10)`
5. Add the **CameraController** component (`Assets/Scripts/Input/CameraController.cs`)
6. Configure CameraController settings:
   - Move Speed: `10`
   - Zoom Speed: `2`
   - Min Zoom: `2`
   - Max Zoom: `50`

### Step 4: Setup UI Manager

1. Create an empty GameObject: **GameObject > Create Empty**
2. Rename it to `UIManager`
3. Add the **UIManager** component (`Assets/Scripts/UI/UIManager.cs`)

### Step 5: Create HUD Canvas

1. Create a Canvas: **GameObject > UI > Canvas**
2. Rename it to `HUD_Canvas`
3. Select the Canvas and set:
   - **Render Mode**: Screen Space - Overlay
   - **Canvas Scaler > UI Scale Mode**: Scale With Screen Size
   - **Reference Resolution**: `1920 x 1080`

### Step 6: Create HUD Controller

1. Create an empty child under `HUD_Canvas`: **Right-click HUD_Canvas > Create Empty**
2. Rename it to `HUD`
3. Add the **HUDController** component (`Assets/Scripts/UI/HUDController.cs`)

### Step 7: Create HUD Elements

#### Game Time Text
1. Create: **Right-click HUD > UI > Text - TextMeshPro**
2. Rename to `GameTimeText`
3. Set RectTransform:
   - Anchor: Top-Left
   - Position: `(10, -10, 0)`
   - Pivot: `(0, 1)`
4. Set TextMeshProUGUI:
   - Text: "Time: 00:00"
   - Font Size: `24`
   - Color: White
   - Alignment: Left, Top

#### Drone Count Text
1. Create: **Right-click HUD > UI > Text - TextMeshPro**
2. Rename to `DroneCountText`
3. Set RectTransform:
   - Anchor: Top-Left
   - Position: `(10, -40, 0)`
   - Pivot: `(0, 1)`
4. Set TextMeshProUGUI:
   - Text: "Drones: 0 / 0"
   - Font Size: `24`
   - Color: White

#### FPS Text
1. Create: **Right-click HUD > UI > Text - TextMeshPro**
2. Rename to `FPSText`
3. Set RectTransform:
   - Anchor: Top-Right
   - Position: `(-10, -10, 0)`
   - Pivot: `(1, 1)`
4. Set TextMeshProUGUI:
   - Text: "FPS: 60"
   - Font Size: `20`
   - Color: Yellow

#### Paused Indicator
1. Create: **Right-click HUD > UI > Text - TextMeshPro**
2. Rename to `PausedIndicator`
3. Set RectTransform:
   - Anchor: Middle-Center
   - Position: `(0, 100, 0)`
   - Size: `(400, 100)`
4. Set TextMeshProUGUI:
   - Text: "PAUSED"
   - Font Size: `48`
   - Font Style: Bold
   - Color: Red
   - Alignment: Center, Middle
5. **Disable the GameObject** (it should be hidden initially)

### Step 8: Connect HUD Elements to HUDController

1. Select the `HUD` GameObject (with HUDController component)
2. In the Inspector, assign the following references:
   - **HUD Root**: Drag the `HUD` GameObject itself
   - **Game Time Text**: Drag `GameTimeText`
   - **Drone Count Text**: Drag `DroneCountText`
   - **FPS Text**: Drag `FPSText`
   - **Paused Indicator**: Drag `PausedIndicator`

### Step 9: Connect HUD to UIManager

1. Select the `UIManager` GameObject
2. In the Inspector, assign:
   - **HUD Controller**: Drag the `HUD` GameObject (with HUDController component)

### Step 10: Add Test Helper

1. Create an empty GameObject: **GameObject > Create Empty**
2. Rename it to `Phase1TestHelper`
3. Add the **Phase1TestHelper** component (`Assets/Scripts/Core/Phase1TestHelper.cs`)
4. In the Inspector, verify:
   - **Test On Start**: Checked (enabled)
   - **Verbose Logging**: Checked (enabled)

### Step 11: Save and Test

1. Save the scene: **Ctrl+S** (Cmd+S on Mac)
2. Press **Play** in Unity Editor

## Expected Results

When you press Play, you should see:

### Console Output
```
=== PHASE 1 FOUNDATION TEST ===
[Phase1Test] Testing all systems...
[Test] Testing GameManager...
  - Instance exists: ✓
  - Current State: Initializing
  - Current Mode: None
  - State change: ✓
GameManager test passed
[Test] Testing TimeManager...
  - Instance exists: ✓
  - Game Time: 0.00s
  - Time Scale: 1x
  - Is Paused: False
  - Pause: ✓
  - Resume: ✓
TimeManager test passed
[Test] Testing EventBus...
  - Subscribed to TestEvent: ✓
  - Event received: Hello from EventBus!
  - Event published and received: ✓
  - Unsubscribed: ✓
  - Unsubscribe verified: ✓
EventBus test passed
[Test] Testing CameraController...
  - Instance exists: ✓
  - Position: (0.0, 0.0, -10.0)
  - Mouse world position: (X, Y, 0.0)
  - GetMouseWorldPosition: ✓
CameraController test passed
[Test] Testing UIManager...
  - Instance exists: ✓
  - HUD exists: ✓
UIManager test passed
✓ ALL TESTS PASSED
```

### On-Screen UI
- Top-left: Game time display (updating every frame)
- Top-left: Drone count (showing 0 / 0)
- Top-right: FPS counter (updating every 0.5s)
- Debug GUI panel on left side with test buttons

### Interactive Tests

Try these controls to verify functionality:

#### Camera Controls
- **WASD** or **Arrow Keys**: Move camera
- **Mouse Wheel**: Zoom in/out
- **Shift + WASD**: Fast camera movement

#### Test Keyboard Shortcuts
- **T**: Run all tests again
- **E**: Test EventBus only
- **G**: Test GameManager only
- **M**: Test TimeManager only

#### Time Controls (Debug - Editor Only)
- **Space**: Pause/Resume game
- **1**: Normal speed (1x)
- **2**: Fast speed (2x)
- **3**: Very fast speed (4x)
- **4**: Slow motion (0.5x)
- **ESC**: Toggle pause (via GameManager)

### Verification Checklist

- [ ] Game time is displayed and updating
- [ ] FPS counter shows current framerate
- [ ] Camera moves with WASD keys
- [ ] Camera zooms with mouse wheel
- [ ] Pressing Space pauses the game (time stops, "PAUSED" appears)
- [ ] Pressing Space again resumes (time continues, "PAUSED" disappears)
- [ ] All system tests pass in console
- [ ] Debug GUI buttons work
- [ ] No errors in console

## Troubleshooting

### "NullReferenceException: Object reference not set to an instance of an object"
- Check that all HUD elements are assigned in HUDController
- Verify UIManager has HUDController reference assigned
- Ensure GameManager exists in scene before UIManager initializes

### "The type or namespace name 'TextMeshPro' could not be found"
- Open **Window > TextMeshPro > Import TMP Essential Resources**
- Add `using TMPro;` at top of any script using TextMeshPro

### Camera doesn't move
- Check CameraController component is attached to Main Camera
- Verify game is not paused (press Space to unpause)
- Check move speed is set to a reasonable value (default: 10)

### HUD doesn't show
- Verify Canvas is set to Screen Space - Overlay
- Check HUD GameObject and all children are active (enabled)
- Ensure Camera has UICamera tag (not required for overlay, but good practice)

### Tests fail
- Read the error message in console carefully
- Check that all references are assigned in Inspector
- Verify no compilation errors exist
- Try restarting Unity Editor

## Next Steps

Once all tests pass and the scene is working correctly:

1. **Save the scene** as a template for future development
2. **Review Phase 1 completion** in console output
3. **Proceed to Phase 2**: Physics System implementation
   - See `ARCHITECTURE.md` for Phase 2 details
   - Next systems: SpaceBody, PhysicsManager, Gravity, Collisions

## Phase 1 Systems Summary

✓ **GameManager** - Singleton lifecycle manager with state machine
✓ **TimeManager** - Game time, pause, time scale controls
✓ **EventBus** - Global publish/subscribe event system
✓ **CameraController** - 2D camera with WASD movement and zoom
✓ **UIManager** - UI panel coordination and state management
✓ **HUDController** - Main gameplay HUD display

**Total Implementation**: ~800 lines of code across 6 core systems

---

**Phase 1: Foundation - Complete ✓**
