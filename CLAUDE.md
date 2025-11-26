# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Asteroid Foundry** is a 2D space automation simulator where players design modular drones for resource mining, factory construction, and creating new drones. The game combines realistic space physics (inertia, momentum, orbital mechanics) with accessible automation through pre-configured drone behaviors.

**Genre:** Space Automation Puzzle / Sandbox Strategy
**Target Platform:** WebGL (PC Browser)
**Unity Version:** 6000.1.3f1 (Unity 6)
**Render Pipeline:** Universal Render Pipeline (URP) 2D

### Key Features
- **Space Physics**: Movement with inertia, gravity, no friction - creates unique puzzles
- **Modular Drones**: Players assemble drones from 15+ module types (cores, thrusters, payloads, etc.)
- **Accessible AI**: Select from pre-made behaviors (Mining, Hauler, Scout, Builder, etc.) instead of programming
- **Two Game Modes**:
  - Mission Mode - 20 puzzle missions introducing mechanics
  - Sandbox Mode - Infinite procedural asteroid field
- **Production Chains**: Mine resources → Process → Build modules → Assemble drones → Expand

### Documentation

**📖 Complete documentation available in:**
- **`GAME_DESIGN.md`** - Full game design document (~80 pages)
  - Game mechanics, progression, balancing
  - All modules, behaviors, resources
  - Mission structure, UI/UX design
- **`ARCHITECTURE.md`** - Technical architecture (~70 pages)
  - System design, code structure
  - Design patterns, data flow
  - Performance optimization strategies
  - Implementation roadmap

**⚠️ Always consult these documents before making architectural decisions.**

## Unity Configuration

- **Unity Version**: 6000.1.3f1 (Unity 6)
- **Target Platform**: WebGL (primary)
- **Render Pipeline**: Universal Render Pipeline (URP) 2D
- **Input System**: New Input System (com.unity.inputsystem)
- **C# Language Version**: C# 9.0
- **Target Framework**: .NET Standard 2.1

## Current Project Status

**Phase:** Pre-Development / Planning
**Status:** Architecture and design documentation complete, ready to begin implementation

**Completed:**
- ✅ Game design document (GAME_DESIGN.md)
- ✅ Technical architecture (ARCHITECTURE.md)
- ✅ Unity project setup (URP 2D, Input System, WebGL)

**Next Steps:** See Implementation Roadmap below

## Project Structure

**Planned structure (from ARCHITECTURE.md):**
```
Assets/
├── Scripts/
│   ├── Core/           - GameManager, TimeManager, EventBus, ServiceLocator
│   ├── Drone/          - Drone entities, modules, behaviors, subsystems
│   ├── Physics/        - Custom space physics (SpaceBody, Gravity, Collisions)
│   ├── Resources/      - Resource management, inventory, production
│   ├── Buildings/      - Structures, placement, production queues
│   ├── World/          - Procedural generation, asteroids, events
│   ├── UI/             - All user interfaces (HUD, Designer, etc.)
│   ├── Mission/        - Mission system, objectives, evaluation
│   ├── Progression/    - Tech tree, research, unlocks
│   ├── Input/          - Input handling, camera, selection
│   ├── SaveSystem/     - Persistence, serialization
│   └── Utilities/      - Object pooling, extensions, helpers
│
├── Data/               - ScriptableObject definitions
│   ├── Modules/        - Drone module definitions
│   ├── Behaviors/      - Behavior configurations
│   ├── Resources/      - Resource types
│   ├── Recipes/        - Production recipes
│   ├── Technologies/   - Tech tree nodes
│   └── Missions/       - Mission definitions
│
├── Scenes/             - Unity scenes
├── Prefabs/            - Game object prefabs
├── Settings/           - Project settings (existing)
├── Materials/          - Materials and shaders
├── Sprites/            - 2D artwork
└── WebGLTemplates/     - PWA template (existing)
```

## Build and Development Commands

### Building the Project
The project uses GitHub Actions for automated builds. Local builds should be done through Unity Editor:
- **Build WebGL**: File → Build Settings → WebGL → Build
- **Build and Run**: File → Build Settings → WebGL → Build And Run

### CI/CD Pipeline
The project uses GitHub Actions (`.github/workflows/main.yml`) to:
1. Build WebGL version on pushes to main branch
2. Deploy to GitHub Pages automatically
3. Cache Library folder for faster builds

### Opening the Project
- Open Unity Hub
- Add project from disk: Select the `Asteroid-Foundry` folder
- Unity will automatically detect version 6000.1.3f1

## Key Packages and Dependencies

**Core Unity Packages:**
- `com.unity.render-pipelines.universal` (17.1.0) - URP rendering
- `com.unity.inputsystem` (1.14.0) - New Input System
- `com.unity.2d.animation` (10.2.0) - 2D animation tools
- `com.unity.2d.sprite` (1.0.0) - 2D sprite support
- `com.unity.2d.spriteshape` (10.1.0) - Spline-based 2D shapes
- `com.unity.ugui` (2.0.0) - UI system
- `com.unity.visualscripting` (1.9.6) - Visual scripting
- `com.unity.splines` (2.8.1) - Spline tools

**Editor Packages:**
- `com.unity.ide.visualstudio` (2.0.23)
- `com.unity.ide.rider` (3.0.36)

## Core Architecture Principles

### Architectural Style
**Layered + Component-Based Architecture**

```
UI Layer (Presentation)
    ↕
Game Logic Layer (8 Core Systems)
    ↕
Data Layer (ScriptableObjects)
    ↕
Infrastructure Layer (Events, Pooling, Utils)
```

### 8 Core Systems

1. **Game Management** - Lifecycle, state coordination
2. **Drone System** - Entity-component-system for drones
3. **Physics System** - Custom space physics (NOT Unity Physics2D)
4. **Resource System** - Inventory, production, mining
5. **Building System** - Structure placement, manufacturing
6. **World System** - Procedural generation, dynamic events
7. **Mission System** - Objectives, progression, evaluation
8. **UI System** - All interfaces (MVP pattern)

**⚠️ Critical:** Use custom physics, not Unity's Physics2D. Space requires inertia without friction.

### Key Design Patterns

| Pattern | Usage |
|---------|-------|
| **Singleton** | GameManager, DroneManager, ResourceManager, UIManager |
| **Factory** | DroneFactory, StructureFactory (creating complex objects) |
| **State Machine** | DroneBehavior (AI), Structure states |
| **Observer (EventBus)** | Inter-system communication, decoupling |
| **Object Pool** | Particles, projectiles, frequently spawned objects |
| **Strategy** | Pathfinding algorithms, different behaviors |

### Data-Driven Design

**All game content defined in ScriptableObjects:**
- Drone modules → `DroneModuleData`
- Behaviors → `BehaviorConfiguration`
- Resources → `ResourceType`
- Recipes → `ProductionRecipe`
- Missions → `Mission`
- Technologies → `Technology`

**Benefits:**
- Designers can balance without code
- Hot-reload in editor
- Easy to test variations

### Custom Space Physics

**Why custom physics?**
Unity's Physics2D assumes ground-based physics with drag/friction. Space requires:
- Inertia-based movement (objects don't stop)
- Impulse-based thrust
- Gravitational influences
- Predictable trajectories for AI

**Key classes:**
- `SpaceBody` - Rigidbody-like component
- `PhysicsManager` - Fixed-timestep simulation
- `GravityField` - Gravity sources (asteroids, planets)
- `TrajectoryPredictor` - Path prediction for AI

### Performance Targets

- **60 FPS** with 100 active drones
- **30 FPS** with 500 drones (acceptable)
- **Build size** <50 MB (WebGL fast loading)

**Optimization strategies:**
- Spatial hashing for collision detection
- Update frequency tiers (distant drones update less)
- LOD (level of detail) for distant objects
- Object pooling for particles/projectiles
- Batch rendering via sprite atlases

## Unity-Specific Notes

### Input System
Input actions defined in `Assets/Settings/InputSystem_Actions.inputactions`.
- Camera movement (WASD)
- Zoom (mouse wheel)
- Selection (LMB, drag, Ctrl/Shift modifiers)
- Pause (Space)
- Groups (1-9, Ctrl+1-9)

### Rendering Pipeline
URP 2D renderer configuration:
- Pipeline asset: `Assets/Settings/UniversalRP.asset`
- 2D renderer: `Assets/Settings/Renderer2D.asset`
- Ensure all materials/shaders are URP 2D compatible

### WebGL Deployment
Custom PWA template at `Assets/WebGLTemplates/PWACustom/`
- Service Worker for offline support
- Progressive Web App manifest
- Optimized for browser constraints

### Assembly Definitions

Planned assembly structure:
- **AsteroidFoundry.Core** - Core systems, managers
- **AsteroidFoundry.Drone** - Drone-related code
- **AsteroidFoundry.Physics** - Custom physics
- **AsteroidFoundry.UI** - All UI code
- **AsteroidFoundry.Common** - Shared utilities

## Code Style

The repository includes an `.editorconfig` at the root level that:
- Sets C# analyzer diagnostic severity to none
- Disables IDE0079 warnings
- Maintains consistent code style across editors

## Implementation Roadmap

### Phase 1: Foundation (Weeks 1-2) - **NEXT**
**Goal:** Core infrastructure

- [ ] Create folder structure
- [ ] Implement GameManager (singleton, lifecycle)
- [ ] Implement TimeManager (pause, time scale)
- [ ] Implement EventBus (publish/subscribe)
- [ ] Basic UI framework setup
- [ ] Camera controller (WASD movement, zoom)

### Phase 2: Physics (Weeks 3-4)
**Goal:** Custom space physics working

- [ ] SpaceBody component (velocity, position, mass)
- [ ] PhysicsManager (fixed timestep simulation)
- [ ] Gravity system (GravityField component)
- [ ] Basic collision detection
- [ ] Trajectory prediction
- [ ] Visual debug tools (draw trajectories)

### Phase 3: Drones (Weeks 5-7)
**Goal:** First playable drone

- [ ] Drone entity (component-based)
- [ ] Module system (base class + thruster, core, fuel)
- [ ] DroneFactory (create drones from blueprints)
- [ ] Manual control (click to select, thrust with keyboard)
- [ ] First AI behavior (MiningBehavior state machine)
- [ ] Fuel and power subsystems

### Phase 4: Resources (Weeks 8-9)
**Goal:** Complete production loop

- [ ] ResourceManager
- [ ] Inventory system
- [ ] ResourceNode (minable asteroids)
- [ ] Production recipes (ScriptableObjects)
- [ ] First structures (Smelter, Storage)
- [ ] Production chains working

### Phase 5: World (Weeks 10-11)
**Goal:** Dynamic game environment

- [ ] Procedural asteroid generation
- [ ] Zone system (Inner/Middle/Outer)
- [ ] Basic dynamic events (solar flare)
- [ ] Lighting/energy zones

### Phase 6: UI (Weeks 12-13)
**Goal:** Complete player interface

- [ ] HUD (resources, drone count)
- [ ] Drone Designer UI
- [ ] Behavior configuration UI
- [ ] Mini-map

### Phase 7: Missions (Weeks 14-15)
**Goal:** Tutorial and missions

- [ ] Mission system
- [ ] Objective tracking
- [ ] First 3 tutorial missions
- [ ] Star rating system

### Phase 8: Polish (Week 16+)
**Goal:** Release-ready

- [ ] Particle effects
- [ ] Sound and music
- [ ] Balancing
- [ ] Performance optimization
- [ ] Testing

## Development Guidelines

### When Adding New Features

1. **Check documentation first**
   - Does GAME_DESIGN.md define this feature?
   - Does ARCHITECTURE.md describe the system?

2. **Follow the architecture**
   - Use appropriate design patterns
   - Maintain separation of concerns
   - Use EventBus for cross-system communication

3. **Data-driven approach**
   - Create ScriptableObjects for configuration
   - Avoid hardcoding values

4. **Test early**
   - Create simple test cases
   - Verify physics behavior
   - Check performance with many objects

### Code Organization

**Naming conventions:**
- Classes: PascalCase (`DroneModule`, `PhysicsManager`)
- Methods: PascalCase (`CalculateGravity()`)
- Private fields: camelCase with underscore (`_currentVelocity`)
- Public properties: PascalCase (`CurrentVelocity`)
- Events: PascalCase with "On" or "Event" (`OnDroneSpawned`, `CollisionEvent`)

**File organization:**
- One class per file
- File name matches class name
- Group related classes in folders

### WebGL Platform Considerations

**Critical constraints:**
- WebGL is **single-threaded** - avoid threading APIs
- Limited memory - test with 100+ drones
- No persistent storage - use browser LocalStorage/IndexedDB
- Slower than native - aggressive optimization needed

**Best practices:**
- Use web-compatible formats (PNG, OGG)
- Enable code stripping (already configured)
- Test build size frequently (target <50 MB)
- Profile in browser, not just editor

### Git Workflow

- **Main branch:** `main` - production-ready code
- **Feature branches:** `feature/feature-name`
- **Current branch:** `draft/relaxed-vibes`
- **CI/CD:** GitHub Actions auto-deploy to GitHub Pages from `main`

**Commit guidelines:**
- Clear, descriptive messages
- Small, focused commits
- Test before committing

## Common Patterns & Examples

### Creating a ScriptableObject

```csharp
[CreateAssetMenu(menuName = "Asteroid Foundry/Module Data")]
public class DroneModuleData : ScriptableObject
{
    public string moduleName;
    public ModuleType moduleType;
    public int slotCount;
    public float mass;
    // ... other properties
}
```

### Using EventBus

```csharp
// Subscribe to event
EventBus.Subscribe<DroneSpawnedEvent>(OnDroneSpawned);

// Publish event
EventBus.Publish(new DroneSpawnedEvent {
    drone = newDrone,
    position = spawnPosition
});

// Unsubscribe (in OnDestroy)
EventBus.Unsubscribe<DroneSpawnedEvent>(OnDroneSpawned);
```

### Singleton Pattern

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

### State Machine for Behaviors

```csharp
public class MiningBehavior : DroneBehavior
{
    enum State { SearchingForAsteroid, FlyingToAsteroid, Mining, ReturningToBase, Unloading }
    State currentState;

    public override void UpdateBehavior(float deltaTime)
    {
        switch (currentState)
        {
            case State.SearchingForAsteroid:
                SearchForTarget();
                break;
            case State.FlyingToAsteroid:
                NavigateToTarget();
                break;
            // ... other states
        }
    }
}
```

## Troubleshooting

### Physics Issues
- **Drones not moving:** Check if PhysicsManager is updating SpaceBody components
- **Weird trajectories:** Verify gravity calculations, check for NaN values
- **Collisions not working:** Ensure SpatialHash is populated

### Performance Problems
- **Low FPS:** Profile with Unity Profiler, check DroneManager update loop
- **Memory leaks:** Verify object pooling, check for event subscription leaks
- **Build size too large:** Review texture compression, enable code stripping

### UI Not Updating
- **HUD frozen:** Check if UI is subscribed to correct events
- **Values wrong:** Verify data source (model) is updating correctly
