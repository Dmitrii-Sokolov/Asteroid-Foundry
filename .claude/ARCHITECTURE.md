# Asteroid Foundry - Technical Architecture

**Version:** 1.0
**Last Updated:** 2025-11-26
**Purpose:** High-level architectural design for implementation

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Core Systems](#core-systems)
4. [Data Flow](#data-flow)
5. [Design Patterns](#design-patterns)
6. [Unity Technologies](#unity-technologies)
7. [Performance Considerations](#performance-considerations)
8. [Module Dependencies](#module-dependencies)

---

## Architecture Overview

### Architectural Style: **Layered + Component-Based**

The game follows a **layered architecture** with clear separation of concerns, combined with Unity's **component-based paradigm**.

```
┌─────────────────────────────────────────────────┐
│          Presentation Layer (UI)                │
│  - HUD, Menus, Drone Designer, Behavior Editor  │
└─────────────────────────────────────────────────┘
                      ↓↑
┌─────────────────────────────────────────────────┐
│         Game Logic Layer (Systems)              │
│  - Drone System, Physics, Resources, Building   │
└─────────────────────────────────────────────────┘
                      ↓↑
┌─────────────────────────────────────────────────┐
│           Data Layer (Models)                   │
│  - ScriptableObjects, Data Structures           │
└─────────────────────────────────────────────────┘
                      ↓↑
┌─────────────────────────────────────────────────┐
│         Infrastructure Layer                    │
│  - Save/Load, Events, Pooling, Utils            │
└─────────────────────────────────────────────────┘
```

### Core Principles

1. **Separation of Concerns** - Each system has a single responsibility
2. **Loose Coupling** - Systems communicate via events/interfaces
3. **Data-Driven Design** - Game content defined in ScriptableObjects
4. **Testability** - Logic separated from MonoBehaviours where possible
5. **Performance First** - Built for 100+ active entities

---

## Project Structure

### Folder Organization

```
Assets/
├── Scripts/
│   ├── Core/                      # Core game systems
│   │   ├── GameManager.cs         # Game lifecycle, scene management
│   │   ├── TimeManager.cs         # Pause, time scale, game speed
│   │   ├── EventBus.cs            # Global event system
│   │   └── ServiceLocator.cs      # Dependency injection alternative
│   │
│   ├── Drone/                     # Everything drone-related
│   │   ├── Drone.cs               # Main drone entity
│   │   ├── DroneFactory.cs        # Creates drones from blueprints
│   │   ├── DroneManager.cs        # Manages all active drones
│   │   ├── DroneBlueprint.cs      # ScriptableObject - drone design
│   │   │
│   │   ├── Modules/               # Drone modules (components)
│   │   │   ├── DroneModule.cs     # Base class
│   │   │   ├── CoreModule.cs      # Hull/chassis
│   │   │   ├── ThrusterModule.cs  # Engines
│   │   │   ├── FuelTankModule.cs  # Fuel storage
│   │   │   ├── PowerModule.cs     # Energy systems
│   │   │   ├── PayloadModule.cs   # Mining, cargo, etc.
│   │   │   └── NavigationModule.cs # Sensors, computer
│   │   │
│   │   ├── Behaviors/             # AI behaviors
│   │   │   ├── DroneBehavior.cs   # Base class (State Machine)
│   │   │   ├── MiningBehavior.cs
│   │   │   ├── HaulerBehavior.cs
│   │   │   ├── ScoutBehavior.cs
│   │   │   ├── BuilderBehavior.cs
│   │   │   ├── RefuelerBehavior.cs
│   │   │   └── RepairBehavior.cs
│   │   │
│   │   └── Systems/               # Drone subsystems
│   │       ├── FuelSystem.cs      # Fuel management
│   │       ├── PowerSystem.cs     # Energy management
│   │       ├── HealthSystem.cs    # Damage & repair
│   │       └── InventorySystem.cs # Cargo management
│   │
│   ├── Physics/                   # Custom physics
│   │   ├── PhysicsManager.cs      # Coordinates physics update
│   │   ├── SpaceBody.cs           # Rigidbody-like for space physics
│   │   ├── GravityField.cs        # Gravity sources (asteroids, planets)
│   │   ├── CollisionSystem.cs     # Collision detection & response
│   │   ├── TrajectoryPredictor.cs # Path prediction
│   │   └── SpatialHash.cs         # Spatial partitioning for optimization
│   │
│   ├── Resources/                 # Resource management
│   │   ├── ResourceManager.cs     # Global resource tracking
│   │   ├── ResourceType.cs        # ScriptableObject - resource definitions
│   │   ├── Inventory.cs           # Resource container
│   │   ├── ResourceNode.cs        # Asteroid with resources
│   │   └── ProductionRecipe.cs    # ScriptableObject - crafting recipes
│   │
│   ├── Buildings/                 # Structures and buildings
│   │   ├── Structure.cs           # Base class for all buildings
│   │   ├── CommandCenter.cs
│   │   ├── Smelter.cs
│   │   ├── Factory.cs
│   │   ├── Shipyard.cs
│   │   ├── Storage.cs
│   │   ├── BuildingPlacer.cs      # Placement system
│   │   └── ProductionQueue.cs     # Manufacturing queue
│   │
│   ├── World/                     # Game world
│   │   ├── AsteroidField.cs       # Procedural generation
│   │   ├── Asteroid.cs            # Individual asteroid
│   │   ├── Star.cs                # Light source
│   │   ├── Zone.cs                # Inner/Middle/Outer zones
│   │   ├── EventManager.cs        # Dynamic events (storms, etc.)
│   │   └── MapGenerator.cs        # Procedural map creation
│   │
│   ├── UI/                        # User Interface
│   │   ├── UIManager.cs           # UI coordination
│   │   ├── HUD/
│   │   │   ├── HUDController.cs
│   │   │   ├── ResourceDisplay.cs
│   │   │   ├── DroneCountDisplay.cs
│   │   │   └── MiniMap.cs
│   │   ├── DroneDesigner/
│   │   │   ├── DesignerUI.cs
│   │   │   ├── ModuleSlot.cs
│   │   │   └── StatsPanel.cs
│   │   ├── BehaviorEditor/
│   │   │   ├── BehaviorEditorUI.cs
│   │   │   └── ParameterControl.cs
│   │   └── Mission/
│   │       ├── MissionUI.cs
│   │       └── ObjectiveDisplay.cs
│   │
│   ├── Mission/                   # Mission system
│   │   ├── MissionManager.cs      # Mission flow control
│   │   ├── Mission.cs             # ScriptableObject - mission definition
│   │   ├── Objective.cs           # Mission objectives
│   │   ├── MissionEvaluator.cs    # Star rating calculation
│   │   └── TutorialSystem.cs      # Interactive tutorials
│   │
│   ├── Progression/               # Tech tree & unlocks
│   │   ├── TechTree.cs            # Technology progression
│   │   ├── Technology.cs          # ScriptableObject - tech definition
│   │   ├── ResearchStation.cs     # Research building
│   │   └── UnlockManager.cs       # Tracks unlocked content
│   │
│   ├── Input/                     # Input handling
│   │   ├── InputManager.cs        # Input System wrapper
│   │   ├── CameraController.cs    # Camera movement
│   │   ├── SelectionManager.cs    # Entity selection
│   │   └── CommandManager.cs      # Issuing orders to drones
│   │
│   ├── SaveSystem/                # Persistence
│   │   ├── SaveManager.cs         # Save/Load coordination
│   │   ├── SaveData.cs            # Serializable game state
│   │   ├── ISaveable.cs           # Interface for saveable objects
│   │   └── BinarySaveStrategy.cs  # Binary serialization
│   │
│   ├── Audio/                     # Sound system
│   │   ├── AudioManager.cs        # Sound playback
│   │   └── MusicManager.cs        # Background music
│   │
│   └── Utilities/                 # Helper classes
│       ├── ObjectPool.cs          # Object pooling
│       ├── Extensions.cs          # Extension methods
│       ├── MathUtils.cs           # Math helpers
│       └── DebugDrawer.cs         # Debug visualization
│
├── Data/                          # ScriptableObject assets
│   ├── Modules/                   # Module definitions
│   ├── Behaviors/                 # Behavior configurations
│   ├── Resources/                 # Resource types
│   ├── Recipes/                   # Production recipes
│   ├── Technologies/              # Tech tree nodes
│   └── Missions/                  # Mission definitions
│
├── Prefabs/                       # Unity prefabs
│   ├── Drones/
│   ├── Buildings/
│   ├── Asteroids/
│   ├── Effects/
│   └── UI/
│
├── Scenes/
│   ├── SampleScene.unity          # Main game scene
│   ├── MainMenu.unity             # Menu
│   └── MissionSelect.unity        # Mission selection
│
├── Settings/                      # Project settings (already exists)
├── Materials/                     # Materials & shaders
├── Sprites/                       # 2D sprites
├── Audio/                         # Sound files
└── Fonts/                         # UI fonts
```

---

## Core Systems

### 1. Game Management System

**Responsibilities:**
- Game lifecycle (init, start, pause, end)
- Scene management
- Global state coordination

**Key Classes:**
```csharp
// Singleton pattern, persistent across scenes
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public GameMode CurrentMode { get; private set; } // Mission or Sandbox

    // System references
    public TimeManager TimeManager { get; private set; }
    public DroneManager DroneManager { get; private set; }
    public ResourceManager ResourceManager { get; private set; }
    public MissionManager MissionManager { get; private set; }

    // Lifecycle
    void InitializeGame();
    void StartMission(Mission mission);
    void StartSandbox(int seed);
    void PauseGame();
    void ResumeGame();
    void EndGame();
}

public enum GameState { Initializing, Running, Paused, GameOver }
public enum GameMode { Mission, Sandbox }
```

**Dependencies:**
- All major systems (initializes them)
- EventBus (for global events)

---

### 2. Drone System

**Responsibilities:**
- Drone lifecycle (spawn, update, destroy)
- Module management
- Behavior execution

**Architecture Pattern:** **Entity-Component-System (Hybrid)**

Each drone is an entity composed of modules (components) with a behavior (system).

```
Drone (Entity)
├── CoreModule (Component) - structure, HP
├── ThrusterModule[] (Component) - movement
├── FuelTankModule[] (Component) - fuel storage
├── PowerModule[] (Component) - energy
├── PayloadModule[] (Component) - function
├── NavigationModule (Component) - sensors
│
├── FuelSystem (Subsystem) - manages fuel consumption
├── PowerSystem (Subsystem) - manages energy
├── HealthSystem (Subsystem) - damage tracking
├── InventorySystem (Subsystem) - cargo
│
└── DroneBehavior (Active component) - AI state machine
```

**Key Classes:**

```csharp
// Main drone entity
public class Drone : MonoBehaviour
{
    public string DroneID { get; private set; }
    public DroneBlueprint Blueprint { get; private set; }

    // Module slots
    public CoreModule Core { get; private set; }
    public List<ThrusterModule> Thrusters { get; private set; }
    public List<FuelTankModule> FuelTanks { get; private set; }
    public List<PowerModule> PowerModules { get; private set; }
    public List<PayloadModule> Payloads { get; private set; }
    public NavigationModule Navigation { get; private set; }

    // Subsystems
    public FuelSystem FuelSystem { get; private set; }
    public PowerSystem PowerSystem { get; private set; }
    public HealthSystem HealthSystem { get; private set; }
    public InventorySystem Inventory { get; private set; }

    // Physics
    public SpaceBody SpaceBody { get; private set; }

    // AI
    public DroneBehavior Behavior { get; private set; }

    // Lifecycle
    void Initialize(DroneBlueprint blueprint);
    void UpdateDrone(float deltaTime); // Called by DroneManager
    void SetBehavior(DroneBehavior behavior);
    void TakeDamage(float amount, DamageType type);
    void Repair(float amount);
    void Destroy();
}

// Factory for creating drones
public class DroneFactory
{
    public Drone CreateDrone(DroneBlueprint blueprint, Vector3 position);
    Drone InstantiateDrone();
    void AssembleModules(Drone drone, DroneBlueprint blueprint);
    void InitializeSystems(Drone drone);
}

// Manages all drones in the game
public class DroneManager : MonoBehaviour
{
    List<Drone> activeDrones;
    Dictionary<string, Drone> droneRegistry; // ID -> Drone

    public Drone SpawnDrone(DroneBlueprint blueprint, Vector3 position);
    public void RemoveDrone(Drone drone);
    public Drone GetDroneByID(string id);
    public List<Drone> GetDronesInRadius(Vector3 center, float radius);

    void Update() {
        // Update all drones (can be optimized later)
        foreach(var drone in activeDrones) {
            drone.UpdateDrone(Time.deltaTime);
        }
    }
}
```

**Module Base Class:**

```csharp
// Base class for all modules
[System.Serializable]
public abstract class DroneModule
{
    public DroneModuleData ModuleData { get; protected set; }
    public float CurrentHealth { get; protected set; }
    public bool IsOperational => CurrentHealth > 0;

    public int SlotCount => ModuleData.SlotCount;
    public float Mass => ModuleData.Mass;
    public float PowerConsumption => ModuleData.PowerConsumption;

    public virtual void Initialize(DroneModuleData data);
    public virtual void Update(float deltaTime);
    public virtual void TakeDamage(float amount);
    public virtual void Repair(float amount);
}

// ScriptableObject defining module stats
[CreateAssetMenu(menuName = "Asteroid Foundry/Module Data")]
public class DroneModuleData : ScriptableObject
{
    public string moduleName;
    public ModuleType moduleType;
    public int slotCount;
    public float mass;
    public float health;
    public float powerConsumption;
    public Sprite icon;
    public ResourceCost[] buildCost;
}
```

**Behavior State Machine:**

```csharp
// Base class for all AI behaviors
public abstract class DroneBehavior
{
    protected Drone drone;
    protected BehaviorState currentState;

    public abstract void Initialize(Drone drone, BehaviorParameters parameters);
    public abstract void UpdateBehavior(float deltaTime);
    public abstract void OnBehaviorStart();
    public abstract void OnBehaviorEnd();

    protected void ChangeState(BehaviorState newState);
}

// Example: Mining behavior
public class MiningBehavior : DroneBehavior
{
    MiningParameters parameters;
    ResourceNode targetAsteroid;
    Structure homeBase;

    enum State {
        SearchingForAsteroid,
        FlyingToAsteroid,
        Mining,
        ReturningToBase,
        Unloading
    }

    public override void UpdateBehavior(float deltaTime)
    {
        switch(currentState) {
            case State.SearchingForAsteroid:
                FindNearestAsteroid();
                break;
            case State.FlyingToAsteroid:
                NavigateToTarget();
                break;
            case State.Mining:
                MineResources();
                break;
            // ... etc
        }
    }
}
```

---

### 3. Physics System

**Responsibilities:**
- Space physics simulation (inertia, no friction)
- Gravity calculations
- Collision detection and response
- Trajectory prediction

**Why Custom Physics?**
Unity's Physics2D assumes ground-based physics. We need:
- No drag/friction
- Accurate impulse-based movement
- Predictable trajectories for AI

**Key Classes:**

```csharp
// Custom "rigidbody" for space physics
public class SpaceBody : MonoBehaviour
{
    // State
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Rotation { get; set; }
    public float AngularVelocity { get; set; }

    // Properties
    public float Mass { get; set; }
    public float Radius { get; set; } // For collision
    public bool IsKinematic { get; set; } // Doesn't move (asteroids)

    // Forces this frame
    Vector2 appliedForce;
    float appliedTorque;

    public void AddForce(Vector2 force);
    public void AddImpulse(Vector2 impulse);
    public void AddTorque(float torque);

    // Called by PhysicsManager
    internal void IntegrateVelocity(float deltaTime);
    internal void IntegratePosition(float deltaTime);
}

// Manages all physics simulation
public class PhysicsManager : MonoBehaviour
{
    List<SpaceBody> bodies;
    List<GravityField> gravityFields;
    SpatialHash spatialHash; // For collision optimization

    // Fixed timestep for determinism
    float fixedDeltaTime = 0.02f; // 50 Hz
    float accumulator = 0f;

    void Update()
    {
        accumulator += Time.deltaTime;

        while(accumulator >= fixedDeltaTime) {
            PhysicsStep(fixedDeltaTime);
            accumulator -= fixedDeltaTime;
        }
    }

    void PhysicsStep(float dt)
    {
        // 1. Apply gravity
        ApplyGravity(dt);

        // 2. Integrate velocities (apply forces)
        foreach(var body in bodies) {
            body.IntegrateVelocity(dt);
        }

        // 3. Detect collisions
        DetectCollisions();

        // 4. Resolve collisions (impulses)
        ResolveCollisions();

        // 5. Integrate positions
        foreach(var body in bodies) {
            body.IntegratePosition(dt);
        }
    }

    void ApplyGravity(float dt)
    {
        foreach(var field in gravityFields) {
            foreach(var body in bodies) {
                if(!body.IsKinematic) {
                    Vector2 force = field.CalculateGravityForce(body);
                    body.AddForce(force);
                }
            }
        }
    }

    void DetectCollisions()
    {
        // Use spatial hash for O(n) instead of O(n²)
        spatialHash.DetectCollisions();
    }
}

// Gravity source (asteroids, planets)
public class GravityField : MonoBehaviour
{
    public float gravitationalMass = 1000f;
    public float influenceRadius = 500f;

    public Vector2 CalculateGravityForce(SpaceBody body)
    {
        Vector2 direction = (Vector2)transform.position - body.Position;
        float distanceSq = direction.sqrMagnitude;

        if(distanceSq > influenceRadius * influenceRadius)
            return Vector2.zero;

        float forceMagnitude = (gravitationalMass * body.Mass) / distanceSq;
        return direction.normalized * forceMagnitude;
    }
}

// Spatial partitioning for collision optimization
public class SpatialHash
{
    Dictionary<Vector2Int, List<SpaceBody>> grid;
    float cellSize;

    public void Insert(SpaceBody body);
    public void Clear();
    public List<SpaceBody> GetNearby(Vector2 position, float radius);
    public void DetectCollisions();
}
```

**Trajectory Prediction:**

```csharp
public class TrajectoryPredictor
{
    // Predict future position accounting for physics
    public Vector2[] PredictTrajectory(
        SpaceBody body,
        Vector2 thrust,
        float duration,
        int steps)
    {
        Vector2[] points = new Vector2[steps];

        // Simulate forward in time
        Vector2 pos = body.Position;
        Vector2 vel = body.Velocity;
        float dt = duration / steps;

        for(int i = 0; i < steps; i++) {
            // Apply thrust
            vel += thrust / body.Mass * dt;

            // Apply gravity
            foreach(var field in gravityFields) {
                Vector2 gravityForce = field.CalculateGravityForce(...);
                vel += gravityForce / body.Mass * dt;
            }

            // Integrate position
            pos += vel * dt;

            points[i] = pos;
        }

        return points;
    }
}
```

---

### 4. Resource System

**Responsibilities:**
- Resource definitions
- Inventory management
- Resource transfer
- Production recipes

**Key Classes:**

```csharp
// Resource type definition
[CreateAssetMenu(menuName = "Asteroid Foundry/Resource Type")]
public class ResourceType : ScriptableObject
{
    public string resourceName;
    public ResourceTier tier; // Raw, Processed, Component, Rare
    public Sprite icon;
    public Color iconColor;
}

public enum ResourceTier { Raw, Processed, Component, Rare }

// Inventory container
public class Inventory
{
    Dictionary<ResourceType, int> resources;
    int maxCapacity;

    public int GetAmount(ResourceType type);
    public bool HasSpace(int amount);
    public bool CanAdd(ResourceType type, int amount);
    public void Add(ResourceType type, int amount);
    public bool CanRemove(ResourceType type, int amount);
    public void Remove(ResourceType type, int amount);
    public bool TryTransferTo(Inventory target, ResourceType type, int amount);
}

// Resource node (asteroid with mineable resources)
public class ResourceNode : MonoBehaviour
{
    public ResourceType resourceType;
    public int totalAmount;
    public int remainingAmount;
    public float miningRate = 10f; // units per second

    public bool CanMine => remainingAmount > 0;

    public int Mine(float deltaTime, float miningPower)
    {
        int mined = Mathf.Min(
            Mathf.FloorToInt(miningRate * miningPower * deltaTime),
            remainingAmount
        );
        remainingAmount -= mined;
        return mined;
    }
}

// Production recipe
[CreateAssetMenu(menuName = "Asteroid Foundry/Recipe")]
public class ProductionRecipe : ScriptableObject
{
    public ResourceCost[] inputs;
    public ResourceCost[] outputs;
    public float productionTime; // seconds
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType type;
    public int amount;
}

// Global resource tracking
public class ResourceManager : MonoBehaviour
{
    // Player's total resources (all bases combined)
    Inventory globalInventory;

    // All resource nodes in the world
    List<ResourceNode> resourceNodes;

    public void RegisterResourceNode(ResourceNode node);
    public List<ResourceNode> FindNodesWithResource(ResourceType type, Vector2 position, float radius);
}
```

---

### 5. Building System

**Responsibilities:**
- Structure placement
- Production management
- Power/fuel distribution

**Key Classes:**

```csharp
// Base class for all structures
public abstract class Structure : MonoBehaviour
{
    public string StructureID { get; protected set; }
    public StructureData Data { get; protected set; }

    public StructureState State { get; protected set; }
    public float CurrentHealth { get; protected set; }

    public Inventory Inventory { get; protected set; }

    public virtual void Initialize(StructureData data);
    public virtual void UpdateStructure(float deltaTime);
    public abstract void OnPlaced();
    public abstract void OnDestroy();
}

public enum StructureState { Constructing, Idle, Active, Damaged, Destroyed }

// Production structure (Smelter, Factory, etc.)
public class ProductionStructure : Structure
{
    ProductionQueue productionQueue;
    float currentProgress;

    public void QueueProduction(ProductionRecipe recipe, int quantity);
    public override void UpdateStructure(float deltaTime)
    {
        if(!productionQueue.HasJobs || State != StructureState.Active)
            return;

        var job = productionQueue.CurrentJob;

        // Check if we have inputs
        if(!HasRequiredInputs(job.recipe))
            return;

        // Produce
        currentProgress += deltaTime;

        if(currentProgress >= job.recipe.productionTime) {
            CompleteProduction(job);
            currentProgress = 0;
            productionQueue.DequeueJob();
        }
    }
}

// Building placement system
public class BuildingPlacer : MonoBehaviour
{
    Structure previewStructure;
    bool isPlacementValid;

    public void StartPlacement(StructureData structureData);
    public void UpdatePlacementPreview(Vector2 mousePosition);
    public bool CanPlaceAt(Vector2 position);
    public void ConfirmPlacement();
    public void CancelPlacement();
}
```

---

### 6. World System

**Responsibilities:**
- Procedural generation
- Asteroid spawning
- Zone management
- Dynamic events

**Key Classes:**

```csharp
// Procedural asteroid field generation
public class AsteroidField : MonoBehaviour
{
    public int seed;
    public int asteroidCount;
    public List<Asteroid> asteroids;

    public void GenerateField(int seed)
    {
        Random.InitState(seed);

        // Generate asteroids in zones
        GenerateInnerZone();
        GenerateMiddleZone();
        GenerateOuterZone();

        // Assign resources
        AssignResourcesToAsteroids();

        // Set orbital velocities
        SetOrbitalMotion();
    }
}

// Individual asteroid
public class Asteroid : MonoBehaviour
{
    public AsteroidType type;
    public float size;
    public List<ResourceNode> resourceNodes;
    public GravityField gravity; // If large enough

    // Orbital motion
    public Vector2 orbitalCenter;
    public float orbitalRadius;
    public float orbitalSpeed;
}

// Dynamic events
public class EventManager : MonoBehaviour
{
    float solarFlareTimer;
    float meteorShowerTimer;
    float rareAsteroidTimer;

    void Update()
    {
        UpdateTimers();

        if(ShouldTriggerSolarFlare())
            TriggerSolarFlare();

        if(ShouldTriggerMeteorShower())
            TriggerMeteorShower();

        if(ShouldSpawnRareAsteroid())
            SpawnRareAsteroid();
    }

    void TriggerSolarFlare()
    {
        EventBus.Publish(new SolarFlareEvent {
            intensity = Random.Range(0.5f, 1.5f),
            duration = Random.Range(10f, 30f)
        });
    }
}
```

---

### 7. Mission System

**Responsibilities:**
- Mission flow
- Objective tracking
- Win/lose conditions
- Star rating

**Key Classes:**

```csharp
// Mission definition
[CreateAssetMenu(menuName = "Asteroid Foundry/Mission")]
public class Mission : ScriptableObject
{
    public string missionName;
    public string description;
    public int missionNumber;

    public MissionType type; // Tutorial, Standard, Challenge
    public Objective[] objectives;
    public MissionConstraints constraints;
    public StarRequirements starRequirements;

    public List<DroneModuleData> unlocksOnCompletion;
}

[System.Serializable]
public class Objective
{
    public ObjectiveType type; // Collect, Build, Survive, etc.
    public string description;
    public ObjectiveParameters parameters;
}

[System.Serializable]
public class MissionConstraints
{
    public float timeLimit; // 0 = no limit
    public int maxDrones; // 0 = no limit
    public int startingResources;
}

// Mission controller
public class MissionManager : MonoBehaviour
{
    Mission currentMission;
    MissionState state;
    float elapsedTime;

    List<Objective> completedObjectives;

    public void StartMission(Mission mission);
    public void UpdateMission(float deltaTime);
    public void CompleteMission();
    public void FailMission();
    public int CalculateStarRating();
}
```

---

### 8. UI System

**Responsibilities:**
- All user interfaces
- Input handling for UI
- HUD updates

**Architecture:** Model-View-Presenter (MVP) pattern

```
Model (Game Logic) ←→ Presenter ←→ View (UI)
```

**Key Classes:**

```csharp
// UI Manager coordinates all UI
public class UIManager : MonoBehaviour
{
    public HUDController HUD { get; private set; }
    public DroneDesignerUI DesignerUI { get; private set; }
    public BehaviorEditorUI BehaviorUI { get; private set; }
    public MissionUI MissionUI { get; private set; }

    Stack<UIPanel> panelStack; // For modal dialogs

    public void ShowPanel(UIPanel panel);
    public void HidePanel(UIPanel panel);
    public void HideAllPanels();
}

// Example: HUD Controller (Presenter)
public class HUDController : MonoBehaviour
{
    // View components
    [SerializeField] ResourceDisplay resourceDisplay;
    [SerializeField] DroneCountDisplay droneCountDisplay;
    [SerializeField] MiniMap miniMap;

    void Update()
    {
        // Update displays from model
        resourceDisplay.UpdateResources(ResourceManager.GlobalInventory);
        droneCountDisplay.UpdateCount(
            DroneManager.ActiveDrones.Count,
            DroneManager.MaxDrones
        );
    }
}
```

---

## Data Flow

### Example: Player builds a drone

```
1. Player designs drone in DesignerUI
   ├─> Selects modules from catalog
   ├─> Views stats preview
   └─> Clicks "Build"

2. DesignerUI validates design
   ├─> Checks module compatibility
   ├─> Calculates total cost
   └─> Checks if player has resources

3. DesignerUI calls DroneFactory.CreateDrone(blueprint)

4. DroneFactory
   ├─> Deducts resources from ResourceManager
   ├─> Instantiates Drone prefab
   ├─> Assembles modules according to blueprint
   ├─> Initializes systems (Fuel, Power, Health, Inventory)
   └─> Returns Drone instance

5. DroneManager.SpawnDrone(drone)
   ├─> Adds to activeDrones list
   ├─> Registers in droneRegistry
   └─> Publishes DroneSpawnedEvent

6. HUD updates drone count (listens to DroneSpawnedEvent)

7. Drone appears in game world
```

### Example: Drone mines asteroid

```
1. MiningBehavior.UpdateBehavior() runs
   └─> Current state: FlyingToAsteroid

2. NavigateToTarget()
   ├─> Calculate desired velocity
   ├─> Calculate thrust vector needed
   ├─> Check if we have fuel/power
   └─> Apply thrust to SpaceBody

3. PhysicsManager.PhysicsStep()
   ├─> Integrates forces (thrust + gravity)
   ├─> Updates velocity
   └─> Updates position

4. Drone arrives at asteroid
   └─> MiningBehavior changes state to Mining

5. State: Mining
   ├─> Get PayloadModule (MiningDrill)
   ├─> Call ResourceNode.Mine(deltaTime, drillingPower)
   ├─> ResourceNode returns mined amount
   ├─> Add to Drone.Inventory
   └─> Consume power/fuel

6. Cargo full?
   └─> MiningBehavior changes state to ReturningToBase

7. Returns to base, unloads to Structure.Inventory

8. ResourceManager updates global inventory

9. HUD updates resource display
```

---

## Design Patterns

### 1. Singleton Pattern
**Used for:** Managers that should have only one instance
**Classes:** GameManager, DroneManager, ResourceManager, UIManager

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

### 2. Factory Pattern
**Used for:** Creating complex objects (Drones, Structures)
**Classes:** DroneFactory, StructureFactory

```csharp
public class DroneFactory
{
    public Drone CreateDrone(DroneBlueprint blueprint, Vector3 position)
    {
        // Complex construction logic
    }
}
```

### 3. State Machine Pattern
**Used for:** Drone behaviors, Mission states
**Classes:** DroneBehavior (base class)

```csharp
public abstract class DroneBehavior
{
    protected enum State { ... }
    protected State currentState;

    protected void ChangeState(State newState)
    {
        OnStateExit(currentState);
        currentState = newState;
        OnStateEnter(newState);
    }
}
```

### 4. Observer Pattern (Event System)
**Used for:** Decoupling systems, UI updates
**Classes:** EventBus

```csharp
public class EventBus
{
    static Dictionary<Type, List<Delegate>> subscribers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        // Add handler to subscriber list
    }

    public static void Publish<T>(T eventData)
    {
        // Invoke all subscribed handlers
    }
}

// Usage:
EventBus.Subscribe<DroneSpawnedEvent>(OnDroneSpawned);
EventBus.Publish(new DroneSpawnedEvent { drone = newDrone });
```

### 5. Object Pool Pattern
**Used for:** Frequently created/destroyed objects (particles, projectiles, UI elements)

```csharp
public class ObjectPool<T> where T : Component
{
    Queue<T> pool;
    T prefab;

    public T Get() { /* Get from pool or instantiate */ }
    public void Return(T obj) { /* Return to pool */ }
}
```

### 6. Command Pattern (Optional)
**Used for:** Undo/redo, replays, networked commands

```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class MoveCommand : ICommand
{
    Drone drone;
    Vector2 targetPosition;

    public void Execute() { /* Move drone */ }
    public void Undo() { /* Revert */ }
}
```

### 7. Strategy Pattern
**Used for:** Different behaviors, pathfinding algorithms

```csharp
public interface IPathfinder
{
    Vector2[] FindPath(Vector2 start, Vector2 end);
}

public class StraightLinePathfinder : IPathfinder { }
public class GravityAssistPathfinder : IPathfinder { }
```

---

## Unity Technologies

### Technologies We'll Use

| Technology | Purpose | Usage |
|------------|---------|-------|
| **Unity 6** | Game engine | Core platform |
| **URP 2D** | Rendering | 2D graphics pipeline |
| **Input System** | Input handling | Player controls |
| **ScriptableObjects** | Data storage | Modules, recipes, missions |
| **Cinemachine** | Camera | Smooth camera following |
| **TextMeshPro** | UI text | All text rendering |
| **DOTween (optional)** | Animation | UI animations, tweening |
| **Unity UI (uGUI)** | User interface | All UI elements |

### Technologies We'll Avoid (for MVP)

| Technology | Why Avoid |
|------------|-----------|
| **DOTS/ECS** | Too complex for initial version, premature optimization |
| **Unity Physics 2D** | Custom physics needed for space |
| **Addressables** | Not needed for WebGL target |
| **Netcode** | No multiplayer in MVP |

### ScriptableObject Architecture

ScriptableObjects store all game data (data-driven design):

```csharp
// Example: Module data
[CreateAssetMenu(menuName = "Asteroid Foundry/Module Data")]
public class DroneModuleData : ScriptableObject
{
    [Header("Identity")]
    public string moduleName;
    public string description;
    public ModuleType type;

    [Header("Stats")]
    public int slotCount;
    public float mass;
    public float health;
    public float powerConsumption;

    [Header("Costs")]
    public ResourceCost[] buildCost;

    [Header("Visuals")]
    public Sprite icon;
    public GameObject prefab;
}
```

**Benefits:**
- Designers can tweak values without touching code
- Easy to balance
- Hot-reload in editor
- Shareable between scenes

---

## Performance Considerations

### Target Performance
- **60 FPS** with 100 drones
- **30 FPS** with 500 drones (acceptable)
- **Build size:** <50 MB (WebGL)

### Optimization Strategies

#### 1. Spatial Partitioning
```csharp
// Grid-based spatial hash for collision detection
// Reduces O(n²) to O(n)
public class SpatialHash
{
    const float CELL_SIZE = 100f;
    Dictionary<Vector2Int, List<SpaceBody>> grid;
}
```

#### 2. Update Frequency Tiers
```csharp
// Not all drones need 60 Hz updates
public class DroneManager
{
    void Update()
    {
        // High priority: Drones in combat, selected, near camera
        UpdateTier1(Time.deltaTime); // Every frame

        // Medium priority: Active drones off-screen
        if(Time.frameCount % 2 == 0)
            UpdateTier2(Time.deltaTime * 2);

        // Low priority: Idle/parked drones
        if(Time.frameCount % 10 == 0)
            UpdateTier3(Time.deltaTime * 10);
    }
}
```

#### 3. Level of Detail (LOD)
```csharp
public class Drone : MonoBehaviour
{
    void UpdateVisuals()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if(distanceToCamera > 100f) {
            // Far: Single sprite, no effects
            spriteRenderer.enabled = true;
            particleSystem.Stop();
        } else {
            // Near: Full detail
            spriteRenderer.enabled = true;
            particleSystem.Play();
        }
    }
}
```

#### 4. Object Pooling
```csharp
// Reuse drones instead of Destroy/Instantiate
public class DronePool
{
    Queue<Drone> inactiveDrones;

    public Drone Get()
    {
        if(inactiveDrones.Count > 0)
            return inactiveDrones.Dequeue();
        return Instantiate(dronePrefab);
    }

    public void Return(Drone drone)
    {
        drone.gameObject.SetActive(false);
        inactiveDrones.Enqueue(drone);
    }
}
```

#### 5. Batch Rendering
```csharp
// Use sprite atlases to batch draw calls
// All drone sprites in one texture atlas
// Unity automatically batches identical materials
```

#### 6. Coroutines for Async Operations
```csharp
// Don't block frame with expensive operations
IEnumerator GenerateAsteroidField()
{
    for(int i = 0; i < asteroidCount; i++) {
        CreateAsteroid(i);

        if(i % 100 == 0)
            yield return null; // Pause every 100 asteroids
    }
}
```

---

## Module Dependencies

### Dependency Graph

```
                    GameManager
                        │
        ┌───────────────┼───────────────┐
        │               │               │
    TimeManager    EventBus      SaveManager
        │               │               │
        │               ▼               │
        │      ┌────────┴────────┐      │
        │      │                 │      │
        ▼      ▼                 ▼      ▼
  DroneManager  ResourceManager  PhysicsManager
        │              │                │
        ├──────────────┼────────────────┤
        │              │                │
        ▼              ▼                ▼
      Drone        Inventory        SpaceBody
        │              │                │
        ├──────────────┘                │
        │                               │
        ▼                               │
  DroneBehavior ←───────────────────────┘
        │
        ▼
  PathfindingSystem
```

### Initialization Order

```csharp
// GameManager.Awake() - initialization sequence
void InitializeGame()
{
    // 1. Infrastructure
    InitializeEventBus();
    InitializeObjectPools();

    // 2. Core systems
    TimeManager.Initialize();
    SaveManager.Initialize();

    // 3. Game systems
    PhysicsManager.Initialize();
    ResourceManager.Initialize();
    DroneManager.Initialize();
    BuildingManager.Initialize();

    // 4. World
    WorldGenerator.Initialize();

    // 5. Mission/Sandbox
    if(gameMode == GameMode.Mission)
        MissionManager.Initialize();
    else
        SandboxManager.Initialize();

    // 6. UI
    UIManager.Initialize();

    // 7. Ready
    currentState = GameState.Running;
    EventBus.Publish(new GameStartedEvent());
}
```

---

## Testing Strategy

### Unit Tests
Test individual systems in isolation:

```csharp
[Test]
public void Inventory_Add_IncreasesAmount()
{
    Inventory inv = new Inventory(100);
    ResourceType iron = Resources.Load<ResourceType>("Iron");

    inv.Add(iron, 50);

    Assert.AreEqual(50, inv.GetAmount(iron));
}

[Test]
public void SpaceBody_AddImpulse_ChangesVelocity()
{
    SpaceBody body = new SpaceBody { Mass = 100 };
    Vector2 impulse = new Vector2(100, 0);

    body.AddImpulse(impulse);
    body.IntegrateVelocity(1f);

    Assert.AreEqual(1f, body.Velocity.x);
}
```

### Integration Tests
Test system interactions:

```csharp
[Test]
public void Drone_Mining_AddsResourceToInventory()
{
    // Setup
    Drone drone = CreateTestDrone();
    ResourceNode asteroid = CreateTestAsteroid(ResourceType.Iron, 100);
    MiningBehavior behavior = new MiningBehavior();

    // Execute
    drone.SetBehavior(behavior);
    drone.UpdateDrone(1f); // 1 second of mining

    // Assert
    Assert.Greater(drone.Inventory.GetAmount(ResourceType.Iron), 0);
}
```

### Playtesting Metrics
Track during development:

```csharp
public class AnalyticsManager
{
    public void TrackEvent(string eventName, Dictionary<string, object> parameters);

    // Examples:
    TrackEvent("DroneBuilt", new { moduleCount, totalCost, timePlayed });
    TrackEvent("MissionCompleted", new { missionID, stars, time, droneCount });
    TrackEvent("GameOver", new { reason, survivalTime });
}
```

---

## Next Steps

### Implementation Phases

**Phase 1: Foundation (Week 1-2)**
- ✅ Project structure setup
- ✅ Core managers (GameManager, TimeManager)
- ✅ EventBus implementation
- ✅ Basic UI framework

**Phase 2: Physics (Week 3-4)**
- ✅ SpaceBody implementation
- ✅ PhysicsManager
- ✅ Gravity system
- ✅ Collision detection
- ✅ Trajectory prediction

**Phase 3: Drones (Week 5-7)**
- ✅ Drone entity
- ✅ Module system
- ✅ DroneFactory
- ✅ Basic behavior (manual control)
- ✅ First AI behavior (Mining)

**Phase 4: Resources (Week 8-9)**
- ✅ ResourceManager
- ✅ Inventory system
- ✅ Resource nodes
- ✅ Production recipes

**Phase 5: World (Week 10-11)**
- ✅ Procedural asteroid generation
- ✅ Zone system
- ✅ Dynamic events

**Phase 6: UI (Week 12-13)**
- ✅ HUD
- ✅ Drone designer
- ✅ Behavior editor

**Phase 7: Missions (Week 14-15)**
- ✅ Mission system
- ✅ First 3 tutorial missions
- ✅ Objective tracking

**Phase 8: Polish (Week 16+)**
- ✅ Visual effects
- ✅ Sound
- ✅ Balancing
- ✅ Optimization

---

## Conclusion

This architecture provides:

✅ **Clear separation of concerns** - Each system has a defined role
✅ **Scalability** - Easy to add new modules, behaviors, resources
✅ **Testability** - Systems can be tested independently
✅ **Maintainability** - Code is organized and documented
✅ **Performance** - Designed with optimization in mind

**Key Takeaways:**
1. **Systems communicate via EventBus** - loose coupling
2. **Data lives in ScriptableObjects** - data-driven design
3. **Custom physics for space** - not Unity Physics2D
4. **Modular drones** - component composition
5. **State machines for AI** - clear behavior logic

Ready to start implementation! 🚀

---

*Last Updated: 2025-11-26*
*Next: Begin Phase 1 - Foundation*
