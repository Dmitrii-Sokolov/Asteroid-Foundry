# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Quick Reference

| Task | Location |
|------|----------|
| Game design details | `.claude/docs/GAME_DESIGN.md` |
| Technical architecture | `.claude/docs/ARCHITECTURE.md` |
| Create ScriptableObject | `Assets/Data/` + see patterns below |
| Add drone module | `Assets/Scripts/Drone/Modules/` |
| Add AI behavior | `Assets/Scripts/Drone/Behaviors/` |
| Physics work | `Assets/Scripts/Physics/` |
| Inter-system events | `EventBus` in `Assets/Scripts/Core/` |
| UI components | `Assets/Scripts/UI/` |

---

## Project Overview

**Asteroid Foundry** - 2D space automation simulator with modular drones, realistic space physics (inertia, gravity), and accessible AI behaviors.

| Property | Value |
|----------|-------|
| Genre | Space Automation / Sandbox Strategy |
| Platform | WebGL (PC Browser) |
| Unity | 6000.1.3f1 (Unity 6) |
| Render Pipeline | URP 2D |
| Input System | New Input System |
| C# Version | 9.0 |

---

## Critical Constraints

**DO NOT:**
1. Use Unity Physics2D - only custom `SpaceBody`/`PhysicsManager`
2. Use `Find()`/`GetComponent()` in `Update()` - cache references
3. Use Threading APIs - WebGL is single-threaded
4. Exceed 50MB build size - compress assets

**ALWAYS:**
1. Use `EventBus` for cross-system communication
2. Use `ScriptableObjects` for game data
3. Use `[SerializeField]` instead of `public` for inspector fields
4. Prefer `UniTask` over coroutines for async (when added)

---

## Architecture

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

1. **Game Management** - GameManager, TimeManager
2. **Drone System** - Entities, modules, behaviors
3. **Physics System** - Custom space physics (NOT Unity Physics2D)
4. **Resource System** - Inventory, production, mining
5. **Building System** - Structures, placement
6. **World System** - Procedural generation, events
7. **Mission System** - Objectives, progression
8. **UI System** - MVP pattern

### Design Patterns

| Pattern | Usage |
|---------|-------|
| Singleton | GameManager, DroneManager, ResourceManager, UIManager |
| Factory | DroneFactory, StructureFactory |
| State Machine | DroneBehavior, Structure states |
| Observer | EventBus for decoupling |
| Object Pool | Particles, projectiles |
| Strategy | Pathfinding algorithms |

---

## Code Patterns

### EventBus Usage

```csharp
// Subscribe
EventBus.Subscribe<DroneSpawnedEvent>(OnDroneSpawned);

// Publish
EventBus.Publish(new DroneSpawnedEvent { drone = newDrone });

// Unsubscribe (in OnDestroy!)
EventBus.Unsubscribe<DroneSpawnedEvent>(OnDroneSpawned);
```

### Singleton Pattern

```csharp
public class SomeManager : MonoBehaviour
{
    public static SomeManager Instance { get; private set; }

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

### ScriptableObject Data

```csharp
[CreateAssetMenu(menuName = "Asteroid Foundry/Module Data")]
public class DroneModuleData : ScriptableObject
{
    [SerializeField] private string _moduleName;
    [SerializeField] private ModuleType _moduleType;
    [SerializeField] private int _slotCount;
    [SerializeField] private float _mass;

    public string ModuleName => _moduleName;
    public ModuleType Type => _moduleType;
    public int SlotCount => _slotCount;
    public float Mass => _mass;
}
```

### State Machine (Behavior)

```csharp
public class MiningBehavior : DroneBehavior
{
    private enum State { Searching, Flying, Mining, Returning, Unloading }
    private State _currentState;

    public override void UpdateBehavior(float deltaTime)
    {
        switch (_currentState)
        {
            case State.Searching: SearchForAsteroid(); break;
            case State.Flying: NavigateToTarget(); break;
            case State.Mining: MineResources(deltaTime); break;
            case State.Returning: ReturnToBase(); break;
            case State.Unloading: UnloadCargo(); break;
        }
    }
}
```

---

## Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Class | PascalCase | `DroneModule` |
| Method | PascalCase | `CalculateGravity()` |
| Private field | _camelCase | `_currentVelocity` |
| Property | PascalCase | `CurrentVelocity` |
| Event | PascalCase + Event | `DroneSpawnedEvent` |
| Constant | UPPER_CASE | `MAX_DRONES` |

---

## Troubleshooting

### Physics
- **Drones not moving**: Check PhysicsManager updates SpaceBody
- **Weird trajectories**: Verify gravity, check for NaN
- **Collisions broken**: Ensure SpatialHash populated

### Performance
- **Low FPS**: Profile DroneManager update loop
- **Memory leaks**: Check event subscriptions, object pooling
- **Large build**: Check texture compression

### UI
- **HUD not updating**: Verify event subscriptions
- **Wrong values**: Check model data source

---

## Documentation

Full documentation in `.claude/docs/`:
- **GAME_DESIGN.md** - Complete game design (~1300 lines)
- **ARCHITECTURE.md** - Technical architecture (~1570 lines)

Consult these before major architectural decisions.
