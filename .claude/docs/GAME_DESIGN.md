# Asteroid Foundry - Game Design Document

**Version:** 1.0
**Last Updated:** 2025-11-26
**Genre:** Space Automation Puzzle / Sandbox Strategy
**Platform:** WebGL (PC Browser)
**Target Audience:** Fans of Factorio, Opus Magnum, SpaceChem, Satisfactory

---

## Executive Summary

**Asteroid Foundry** is a 2D space automation simulator where players design modular drones for resource mining, factory construction, and creating new drones. The game's uniqueness lies in **realistic space physics** (inertia, momentum) combined with an **accessible automation system** through selecting pre-made behaviors.

### Core Concept (Elevator Pitch)
> "Imagine Factorio in space, where every unit is a drone you assembled that moves according to physics laws, and the goal is to create a self-replicating army of miners"

---

## Unique Selling Points (USP)

### 🚀 1. Space Physics as Gameplay
- **Inertia**: Drones don't stop instantly, movement requires planning
- **Impulse Control**: Short engine bursts for maneuvers
- **Gravitational Influence**: Large asteroids and planets attract objects
- **Collisions**: Drones can crash into each other and asteroids

**Why it matters**: Creates unique puzzles where you need to think about trajectories, not just "point A → point B"

### 🔧 2. Modular Drone Construction
Each drone is assembled from modules:
- **Core** (determines size and durability)
- **Thrusters** (thrust and maneuverability)
- **Fuel Tanks** (range)
- **Power Systems** (solar panels / reactor / batteries)
- **Payload** (mining tool / cargo hold / construction module)
- **Navigation** (sensors, detection range)

**Why it matters**: Each task requires its own drone design, no universal solution

### 🤖 3. Accessible Automation
Instead of programming - **behavior selection from list** + **parameter tuning**:

**Behavior Examples:**
- `Mining Drone` - "Fly to nearest asteroid with resource X, mine until full, return to base"
- `Hauler Drone` - "Shuttle between point A and B, transporting resources"
- `Scout Drone` - "Patrol zone, scan new asteroids"
- `Builder Drone` - "Deliver modules to coordinates, construct structure"
- `Refueler Drone` - "Refuel drones with low fuel in radius X"

**Behavior Settings:**
- Resource priorities (what to mine first)
- Thresholds (return when fuel <30%, cargo >80%)
- Action zones (work only in area X)
- Emergency protocols (what to do when damaged)

**Why it matters**: Low barrier to entry, but enough depth for optimization

### 🌌 4. Dynamic Playing Field
- **Rotating asteroid belt**: Field constantly changes
- **Orbital mechanics**: Objects move in elliptical orbits
- **Periodic events**: Meteor showers, solar flares, rare asteroids
- **Zones**: Inner (lots of energy, few resources) and outer (lots of resources, little energy)

**Why it matters**: Game doesn't become static, requires strategy adaptation

---

## Game Modes

### 📋 Mission Mode (Campaign Mode)

**Concept**: Series of 15-20 puzzles of increasing difficulty, each introducing a new mechanic

**Progression Structure:**

#### Chapter 1: Basics (Missions 1-5)
1. **"First Flight"** - Build a drone with engine, fly to asteroid
2. **"Iron Mining"** - Add mining module, collect 100 iron
3. **"Fuel Economy"** - Complete task with limited fuel (learn to use inertia)
4. **"Two Drones"** - Control two drones simultaneously, don't let them collide
5. **"Automation"** - Set up `Mining Drone` behavior, let it work autonomously

#### Chapter 2: Logistics (Missions 6-10)
6. **"Transport Chain"** - Miner → Hauler → Base
7. **"Processing"** - Build factory, convert ore to metal
8. **"Specialization"** - Design fast scout and slow hauler
9. **"Gravity"** - Level with planet, use gravity assist
10. **"Self-Replication"** - Build a drone that creates a copy of itself

#### Chapter 3: Complex Systems (Missions 11-15)
11. **"Energy Crisis"** - Work in low-light zone
12. **"Meteor Storm"** - Avoid hazards, use shelters
13. **"Rare Resources"** - Find and mine rare metal from moving asteroid
14. **"Production Chain"** - Iron → Steel → Composite → Drone (multi-stage)
15. **"Final Exam"** - Open task: produce 10 advanced drones

#### Chapter 4: Mastery (Missions 16-20)
- Complex optimization puzzles
- Limited resources
- Combination of all mechanics

**Mission Rating System:**
- ⭐ **1 star** - Complete objective
- ⭐⭐ **2 stars** - Meet time limit
- ⭐⭐⭐ **3 stars** - Use minimum resources/drones

**Rewards:**
- Unlock new modules
- Unlock new drone behaviors
- Unlock sandbox mode (after mission 10)

---

### 🏖️ Sandbox Mode

**Concept**: Infinite procedurally-generated asteroid field, goal - maximum automation

**Starting Conditions:**
- Base station with limited resources
- Small amount of starting fuel and materials
- Access to basic modules (advanced ones unlock through research)

**Game Loop:**
```
Mine basic resources → Build processing →
Produce modules → Assemble drones → Expand operations →
Research rare resources → Advanced technologies →
Scale production
```

**Progression:**
- **Early game (0-30 min)**: Manual control of 2-3 drones, gathering iron/water
- **Mid game (30-90 min)**: First automatic chains, 10-20 drones
- **Late game (90+ min)**: Hundreds of drones, complex production chains, expeditions for rare resources

**Procedural Map Generation:**
```
[Central Zone]
├─ Lots of sunlight (energy)
├─ Basic resources (iron, ice)
└─ Starting base

[Middle Belt]
├─ Moderate lighting
├─ More asteroids
├─ Diverse resources (titanium, copper)
└─ Gravitational anomalies

[Outer Belt]
├─ Weak lighting (need batteries/reactors)
├─ Dangerous zones (meteors)
├─ Rare resources (platinum, iridium)
└─ Unique asteroids
```

**Sandbox Goals (optional):**
- Achieve production of N drones per minute
- Create fully autonomous colony
- Mine all resource types
- Build megastructure (space station)

---

## Core Mechanics

### 1️⃣ Physics and Movement

#### Movement System
- **Inertia**: `velocity = velocity + thrust * deltaTime`
- **Friction**: None (space), drone moves until braking command
- **Mass**: Depends on modules and cargo
- **Thrust**: Determined by quantity and type of engines
- **Acceleration**: `acceleration = thrust / mass`

#### Fuel System
- Each maneuver consumes fuel
- Different engines have different efficiency (ISP - specific impulse)
- Fuel has mass (full tank heavier than empty)
- Fuel sources:
  - **Water ice** → Electrolysis → Hydrogen/Oxygen (cheap, low efficiency)
  - **Chemical fuel** → Produced in factories (expensive, high efficiency)

#### Gravity
- Asteroids >1000m have gravitational field
- `gravity_force = G * (m1 * m2) / distance²`
- Can be used to save fuel (gravity slingshot)
- Danger: can fall onto asteroid

#### Collisions
- **Light touch** (<1 m/s): Bounce, minimal damage
- **Collision** (1-5 m/s): Module damage, cargo loss
- **Catastrophe** (>5 m/s): Drone destruction

---

### 2️⃣ Modular Drone System

#### Module Categories

**A) Core**
Determines size and durability, has slots for other modules

| Core | Slots | Durability | Mass | Cost |
|------|-------|------------|------|------|
| Small | 4 | 50 HP | 100 kg | 10 iron |
| Medium | 8 | 150 HP | 250 kg | 30 iron, 10 copper |
| Large | 16 | 400 HP | 600 kg | 50 iron, 30 titanium |

**B) Thrusters**
Create thrust for movement

| Thruster | Thrust | Fuel Consumption | Slots | Cost |
|----------|--------|------------------|-------|------|
| Ion | 10 N | 0.1/s | 1 | 5 iron, 2 electronics |
| Chemical | 50 N | 1.0/s | 2 | 10 iron, 5 fuel |
| Plasma | 100 N | 0.5/s | 3 | 20 titanium, 10 electronics |

**C) Fuel Systems**

| Module | Capacity | Mass | Slots |
|--------|----------|------|-------|
| Small Tank | 50 units | 20 kg | 1 |
| Large Tank | 200 units | 60 kg | 3 |

**D) Power Systems**

| Module | Output | Condition | Slots |
|--------|--------|-----------|-------|
| Solar Panel | 10 W | Illumination >50% | 1 |
| Battery | 100 W·h | Storage | 1 |
| Reactor | 50 W | Requires fuel | 2 |

**E) Payload**

| Module | Function | Slots |
|--------|----------|-------|
| Drill | Mining 10 u/s | 2 |
| Cargo Hold | Stores 100 u | 2 |
| Construction Module | Places structures | 3 |
| Scanner | Detects resources | 1 |
| Repair Kit | Fixes other drones | 2 |
| Assembly Platform | Creates modules | 4 |

**F) Navigation**

| Module | Function | Slots |
|--------|----------|-------|
| Basic Computer | Simple commands | 1 |
| Advanced AI | Complex behaviors | 2 |
| Sensor Array | Extended range | 1 |

#### Slot System
Each core has limited slots. Player must choose:
- More engines = faster, but less cargo
- More tanks = longer range, but slower
- More weapons = defense, but more expensive

**Design Examples:**

```
[Fast Scout]
Core: Small (4 slots)
- 2× Ion Thruster (2 slots)
- 1× Scanner (1 slot)
- 1× Battery (1 slot)
Result: Fast, long operational time, but can't mine

[Heavy Hauler]
Core: Large (16 slots)
- 4× Chemical Thruster (8 slots)
- 2× Large Tank (6 slots)
- 1× Cargo Hold (2 slots)
Result: Slow, but carries lots, long range

[Autonomous Miner]
Core: Medium (8 slots)
- 2× Ion Thruster (2 slots)
- 1× Solar Panel (1 slot)
- 1× Small Tank (1 slot)
- 1× Drill (2 slots)
- 1× Cargo Hold (2 slots)
Result: Balanced, works forever in sunlight
```

---

### 3️⃣ Drone Behavior System (AI Behaviors)

#### Basic Behaviors (available from start)

**🔨 Mining Drone**
```
Parameters:
- Target Resource: [Resource List] (priority)
- Search Radius: [100-1000m]
- Return Threshold: [0-100%] cargo
- Fuel Reserve: [0-100%] fuel for return
- Home Base: [Coordinates/Structure]

Logic:
1. Find nearest asteroid with needed resource
2. Fly to asteroid (trajectory optimization)
3. Mine until hold full or fuel < Reserve
4. Return to base
5. Unload
6. Refuel (if needed)
7. GOTO 1
```

**🚚 Hauler Drone**
```
Parameters:
- Pickup Point: [Coordinates/Structure]
- Delivery Point: [Coordinates/Structure]
- Cargo Type: [Resource or "Any"]
- Wait for Full Load: [Yes/No]

Logic:
1. Fly to pickup point
2. Load resources
3. Fly to delivery point
4. Unload
5. GOTO 1
```

**🔍 Scout Drone**
```
Parameters:
- Patrol Route: [Waypoint List]
- Scan for: [Asteroid Types]
- Alert Range: [Distance to base for signal]

Logic:
1. Patrol along route
2. Scan asteroids
3. When rare resource found - mark on map
4. GOTO 1
```

#### Advanced Behaviors (unlock through progression)

**🏗️ Builder Drone**
```
Parameters:
- Blueprint: [Structure Type]
- Build Location: [Coordinates]
- Resource Source: [Where to get materials]

Logic:
1. Check material availability
2. Load needed resources
3. Fly to build site
4. Place structure
5. Wait for next task
```

**⛽ Refueler Drone**
```
Parameters:
- Service Radius: [Service radius]
- Fuel Threshold: [% fuel to refuel]
- Priority: [Which drone types prioritized]

Logic:
1. Scan drones in radius
2. Find drones with fuel < Threshold
3. Fly to drone
4. Transfer fuel
5. GOTO 1
```

**🔧 Repair Drone**
```
Parameters:
- Service Radius: [Radius]
- Health Threshold: [% HP to repair]
- Carry Spare Parts: [Yes/No]

Logic:
1. Find damaged drones
2. Fly to drone
3. Repair
4. GOTO 1
```

**🤖 Swarm Coordinator**
```
Parameters:
- Manage Drones: [Type List]
- Load Balancing: [Task Distribution]
- Emergency Protocol: [Actions during danger]

Logic:
1. Analyze subordinate drone efficiency
2. Redistribute tasks
3. Replace inefficient drones
4. GOTO 1
```

#### Emergency Protocols

Activate automatically in critical situations:

- **Low Fuel**: Immediate return to base, ignoring current task
- **Low Health**: Find repair drone or return to station
- **Collision Warning**: Avoid obstacles
- **Lost Signal**: Return to last known base position

---

### 4️⃣ Resources and Production

#### Resource Types

**Tier 1: Raw Materials**
| Resource | Where Found | Use |
|----------|-------------|-----|
| 🪨 Iron Ore | Asteroids (70%) | Basic construction |
| 💧 Water Ice | Comets (40%) | Fuel, cooling |
| 🔶 Copper Ore | Asteroids (30%) | Electronics |
| ⚫ Carbon | Rare asteroids (20%) | Composites |

**Tier 2: Processed Materials**
| Material | Recipe | Use |
|----------|---------|-----|
| 🔩 Iron | 2 ore → 1 iron | Drone modules |
| ⚡ Copper | 2 ore → 1 copper | Electronics |
| ⛽ Fuel | 1 ice → 1 fuel | Engines |
| 🧱 Steel | 2 iron + 1 carbon → 1 steel | Advanced cores |

**Tier 3: Components**
| Component | Recipe | Use |
|-----------|---------|-----|
| 💾 Electronics | 2 copper + 1 silicon → 1 electronics | AI modules |
| ⚙️ Mechanics | 2 iron + 1 copper → 1 mechanics | Engines |
| 🔋 Power Cells | 1 lithium + 2 copper → 1 cell | Batteries |
| 🏗️ Structural Element | 2 steel + 1 titanium → 1 element | Large cores |

**Tier 4: Rare Materials**
| Material | Source | Use |
|----------|--------|-----|
| ⭐ Titanium | Outer belt (10%) | Advanced modules |
| 💎 Platinum | Unique asteroids (5%) | Efficient engines |
| 🌟 Iridium | Events (2%) | Reactors |
| 🔮 Exotic Matter | Special zones (1%) | Experimental tech |

#### Production Structures

**🏭 Primary Factories**

| Structure | Function | Inputs | Outputs | Time |
|-----------|----------|--------|---------|------|
| Smelter | Ore processing | Ore | Metal | 5 sec |
| Factory | Component production | Metals | Components | 10 sec |
| Shipyard | Drone assembly | Modules | Drone | 30 sec |
| Electrolyzer | Fuel production | Ice | Fuel | 3 sec |
| Storage | Storage | - | - | - |

**🔬 Research Structures**

| Structure | Function |
|-----------|----------|
| Laboratory | Research new technologies |
| Scanning Station | Analyze asteroids |
| Prototype Workshop | Create experimental modules |

#### Production Chains (examples)

**Simple Chain: Miner**
```
Asteroid (iron ore)
  → Mining Drone
  → Smelter
  → Iron
  → Factory
  → Components
  → Shipyard
  → New Mining Drone
```

**Complex Chain: Advanced Drone**
```
[Branch 1: Core]
Titanium Ore → Smelter → Titanium
Iron Ore → Smelter → Iron
Titanium + Iron + Carbon → Factory → Structural Element → Core

[Branch 2: Engines]
Platinum → Smelter → Platinum Plates
Copper → Electronics
Platinum + Electronics → Factory → Plasma Engine

[Branch 3: AI]
Silicon + Copper → Factory → Processor
Processor × 5 → Factory → Advanced Computer

[Final]
Core + Engines + AI + Other → Shipyard → Advanced Drone
```

---

### 5️⃣ Energy System

#### Energy Sources

**☀️ Solar Energy**
- Efficiency depends on distance from star
- Formula: `power = base_power * (1 - distance/max_distance)`
- **Map Center**: 100% efficiency
- **Middle Belt**: 50% efficiency
- **Outer Belt**: 10% efficiency
- **Planet Shadow**: 0% efficiency

**🔋 Batteries**
- Store energy for working in shadow
- Recharge in sunlight or from stations
- Capacity limited by module size

**⚛️ Reactors**
- Constant output regardless of lighting
- Require fuel (iridium or composite fuel)
- Heavy and expensive

#### Energy Consumption

Each drone system consumes energy:
- **Thrusters**: 5-20 W (when operating)
- **Drilling**: 10 W (constant)
- **Computer**: 2-8 W (constant)
- **Sensors**: 1-5 W (when scanning)
- **Construction**: 15 W (when operating)

**Energy Balance:**
- If consumption > production → battery drain
- Batteries empty → system shutdown (priority: sensors → mining → movement → computer)
- Critical discharge → drone becomes drifting debris

#### Energy Supply Strategies

**Early game:**
- Solar panels + small batteries
- Work only in lit zones

**Mid game:**
- Hybrid systems (solar + batteries)
- Energy stations for charging

**Late game:**
- Reactors for deep space operations
- Energy network (wireless transfer?)

---

### 6️⃣ Game Field and Environment

#### Space Structure

**🌟 Central Star**
- Source of light and energy
- Danger zone (drone overheating)
- Inaccessible for direct flight

**🪐 Inner Zone (0-500 units from center)**
- Lots of sunlight
- Few asteroids
- Basic resources (iron, ice)
- Ideal for first bases

**🌑 Middle Belt (500-1500 units)**
- Main playing field
- Dense asteroid belt
- Diverse resources
- Moving objects

**🌌 Outer Zone (1500-3000 units)**
- Weak lighting
- Rare but valuable resources
- Dangerous phenomena
- Requires advanced technology

#### Asteroid Types

| Type | Size | Resources | Gravity | Features |
|------|------|-----------|---------|----------|
| Small | 10-50m | 50-200 u | None | Quickly depleted |
| Medium | 50-200m | 500-1000 u | Weak | Standard |
| Large | 200-1000m | 2000-5000 u | Medium | Can build bases |
| Giant | >1000m | 10000+ u | Strong | Dangerous but profitable |
| Comet | 20-100m | Ice | None | Fast-moving |
| Metallic | Various | Pure metals | Medium | Rare, valuable |

#### Dynamic Events

**⚡ Solar Flare** (every 10-15 min)
- Damages drone electronics
- Overloads batteries
- Temporarily increases solar panel output
- Need to hide behind asteroids or shut down systems

**☄️ Meteor Shower** (random)
- Stream of fast small objects
- Dangerous for drones
- Brings additional resources

**🌠 Rare Asteroid** (every 20-30 min)
- Flies through map
- Contains unique resources
- Limited time for mining

**🕳️ Gravitational Anomaly** (rare)
- Temporary gravity increase in zone
- Can pull drones
- Or use for acceleration

---

### 7️⃣ Structures and Bases

#### Basic Structures

**🏠 Command Center**
- Starting structure
- Drone control
- Resource storage (500 u)
- Refueling and repair

**🏭 Production Modules**
- Smelter
- Factory
- Shipyard
- Electrolyzer
- Each takes space, requires energy

**📦 Storage**
- Small Storage (1000 u)
- Large Storage (5000 u)
- Specialized (fuel only/ore only)

**🔌 Power Structures**
- Solar Farm (large panel)
- Battery Bank (energy buffer)
- Reactor Station (powerful source)

#### Advanced Structures

**🔬 Research Complex**
- Unlock new technologies
- Analyze asteroids
- Upgrade modules

**📡 Communication Tower**
- Extends control radius
- Allows coordinating more drones
- Data transfer between bases

**🛡️ Defense Systems**
- Shield (meteor protection)
- Detector (early warning)
- Auto Turret (future: PvP or enemies?)

**🏗️ Construction Platform**
- Automatic structure placement
- Repair damaged buildings
- Base expansion

#### Base Placement

**Strategic Positions:**
- **Central Base**: Near start, lots of energy
- **Mining Outpost**: On large asteroid with resources
- **Deep Station**: In outer belt for rare resources
- **Logistics Hub**: Between zones for transit

**Placement Rules:**
- Can't build too close to star
- Can't build on moving asteroids (or can, but base moves with it?)
- Requires flat surface on large asteroid

---

### 8️⃣ Progression and Technologies

#### Technology Tree

**Tier 1: Basics**
- ✅ Basic Thrusters
- ✅ Small Cores
- ✅ Solar Panels
- ✅ Mining Tool (level 1)
- ✅ Simple Computer

**Tier 2: Automation**
- 🔒 Mining Drone Behavior
- 🔒 Hauler Drone Behavior
- 🔒 Medium Cores
- 🔒 Batteries
- 🔒 Mining Tool (level 2 - faster)

**Tier 3: Efficiency**
- 🔒 Ion Thrusters (economical)
- 🔒 Large Cores
- 🔒 Advanced Computer
- 🔒 Scout Drone Behavior
- 🔒 Scanning Modules

**Tier 4: Expansion**
- 🔒 Builder Drone Behavior
- 🔒 Construction Modules
- 🔒 Reactors
- 🔒 Communication Tower
- 🔒 Long-Range Sensors

**Tier 5: Optimization**
- 🔒 Plasma Thrusters
- 🔒 Refueler Drone Behavior
- 🔒 Repair Drone Behavior
- 🔒 Automated Factory (production without control)

**Tier 6: Mastery**
- 🔒 Swarm Coordinator Behavior
- 🔒 Composite Materials
- 🔒 Advanced Shipyard (faster builds)
- 🔒 Megastructures

#### Research System

**How to unlock technologies:**

**In Mission Mode:**
- Automatically when completing levels
- 3 stars on mission = bonus technology

**In Sandbox Mode:**
- Build research laboratory
- Spend resources + time
- Some require samples of rare materials

**Approximate Research Costs:**
```
Tier 2: 100 iron, 50 copper, 5 minutes
Tier 3: 500 iron, 200 copper, 100 titanium, 15 minutes
Tier 4: 1000 steel, 500 electronics, 30 minutes
Tier 5: 2000 composites, 1000 electronics, 50 platinum, 60 minutes
```

---

### 9️⃣ Controls and Interface (UI/UX)

#### Main Screens

**🖥️ Main Game Field**
- Top-down view (2D)
- Camera: free movement (WASD), zoom (mouse wheel)
- Display layers:
  - Asteroids and structures
  - Drones (color-coded by status)
  - Movement trajectories (10-second prediction)
  - Illumination zones
  - Gravitational fields

**🎨 Drone Color Coding:**
- 🟢 Green: Operating normally
- 🟡 Yellow: Low fuel (<30%)
- 🟠 Orange: Low energy / Overload
- 🔴 Red: Damaged or critical situation
- 🔵 Blue: Idle mode
- ⚪ Gray: Disabled / No signal

**🛠️ Drone Constructor Panel**
- Left: Available module list (categories)
- Center: 3D drone preview (rotation)
- Right: Characteristics
  - Mass
  - Thrust
  - Range
  - Cargo capacity
  - Build cost
- Bottom: "Build", "Save Design", "Cancel" buttons

**🤖 Behavior Configuration Panel**
- Behavior type selection (dropdown)
- Parameters (sliders, input fields, checkboxes)
- Route preview on map (if applicable)
- Priorities and conditions
- "Apply" button

**📊 Information Panel (HUD)**
- Top left:
  - Active drones count / limit
  - FPS (optional)
- Top right:
  - Current resources (icons + numbers)
  - Production rate (resources per minute)
- Bottom:
  - Mini-map
  - Notifications / Alerts
  - Quick access buttons

**📈 Statistics Screen**
- Resource production graphs
- Drone efficiency
- Heat map (most activity areas)
- Event history

**🎯 Mission Screen (mission mode)**
- Objective description
- Current progress
- Constraints (time, resources)
- Rating (stars)

#### Controls

**Keyboard and Mouse:**
- **WASD / Arrows**: Camera movement
- **Mouse Wheel**: Zoom
- **LMB**: Select drone/structure
- **RMB**: Context menu
- **LMB + Drag**: Box selection
- **Ctrl + LMB**: Multiple selection
- **Shift + LMB**: Add to selection
- **Space**: Pause
- **1-9**: Drone groups (assign with Ctrl+1-9)
- **Tab**: Switch between drones
- **Escape**: Menu / Cancel

**Gamepad (optional):**
- Left Stick: Camera
- Right Stick: Cursor
- Triggers: Zoom
- A/X: Select
- B/Circle: Cancel
- D-pad: Quick menu switching

#### Hints and Tutorial

**Tutorial (first missions):**
- Interactive hints with arrows
- Highlight needed buttons
- Block other actions until completion
- Voice assistant (text) "ALISA AI"

**Context Hints:**
- Hover over module → tooltip with characteristics
- Warnings for non-optimal design
  - "⚠️ Insufficient fuel to return to base"
  - "⚠️ Mass too high, thrust insufficient"

**Reference (encyclopedia):**
- All modules with descriptions
- All behaviors with examples
- Production chains
- Optimization tips

---

### 🔟 Visual Style

#### Art Direction

**Overall Aesthetic:**
- **Minimalist sci-fi**
- Clean geometric shapes
- Bright, contrasting colors on dark space background
- Stylization over realism (like FTL or Into the Breach)

**Palette:**
- **Background**: Deep space (#0a0e27), stars - bright dots
- **Asteroids**: Gray (#4a5568), brown (#6b4423) with colored resource veins
- **Drones**: Bright blue (#3b82f6) with type-based accents (green for miners, orange for haulers)
- **Structures**: White (#e5e7eb) with neon accents (#8b5cf6)
- **UI**: Dark blue panels (#1e293b) with blue (#60a5fa) borders

**Animation:**
- Smooth drone movement trajectories
- Engine exhaust particles (vary by engine type)
- Pulsing working structures
- Sparks during mining
- Light effects from welding during construction

**Sound:**
- Ambient space music (Stellaris style)
- Event sound signals (alerts, construction completion)
- Engine hum (varies by type)
- Drilling sound
- UI sounds (clicks, swoops)

---

## Technical Design

### Architecture (high level)

#### Core Systems

**1. Physics System**
- Calculate movement of all objects
- Gravity
- Collisions
- Optimization: spatial hashing for collisions

**2. Drone System**
- Component system (modules)
- AI behaviors (state machine)
- Fuel/Energy management
- Pathfinding (A* accounting for inertia)

**3. Resource System**
- Inventory management
- Production chains
- Asteroid mining
- Transport between structures

**4. Building System**
- Structure placement
- Production queues
- Upgrade system

**5. Event System**
- Solar flares
- Meteors
- Rare asteroids
- Player alerts

**6. Save/Load System**
- Game state serialization
- Cloud saves (optional)
- Auto-save every 5 minutes

**7. UI System**
- Reactive interface (Unity UI Toolkit or UGUI)
- Modal windows
- Module drag & drop

#### Key Classes/Components (approximate structure)

```
Core/
├── GameManager (singleton)
├── TimeManager (pause, speed)
├── SaveManager
└── EventBus (system communication)

Drones/
├── Drone (main class)
├── DroneModule (base module class)
│   ├── ThrusterModule
│   ├── FuelTankModule
│   ├── PowerModule
│   ├── PayloadModule
│   └── NavigationModule
├── DroneBehavior (base behavior class)
│   ├── MiningBehavior
│   ├── HaulerBehavior
│   ├── ScoutBehavior
│   └── ...
├── DroneFactory (drone creation)
└── DroneManager (manage all drones)

Physics/
├── PhysicsEngine (custom or Unity Physics2D)
├── GravityField
├── CollisionDetector
└── TrajectoryPredictor

Resources/
├── Resource (enum or ScriptableObject)
├── Inventory
├── ResourceNode (asteroid with resources)
└── ProductionRecipe

Buildings/
├── Structure (base class)
│   ├── CommandCenter
│   ├── Smelter
│   ├── Factory
│   └── ...
├── BuildingPlacer
└── ProductionQueue

World/
├── AsteroidField (procedural generation)
├── Asteroid
├── Star (light source)
└── EventManager (dynamic events)

UI/
├── UIManager
├── DroneDesignerUI
├── BehaviorEditorUI
├── HUDUI
└── MissionUI
```

#### Design Patterns

**Entity Component System (ECS) - optional**
- For optimizing large numbers of drones
- Unity DOTS (Data-Oriented Technology Stack)
- Alternative: classic OOP with good optimization

**Factory Pattern**
- DroneFactory for creating drones from designs
- StructureFactory for buildings

**State Machine**
- For drone AI behaviors
- For structure states (working/idle/damaged)

**Observer Pattern / Event Bus**
- Event notifications (drone damaged, resource mined)
- UI updates

**Command Pattern**
- System for giving orders to drones
- Undo/redo actions (optional)

**Object Pooling**
- For frequently created objects (particles, meteor projectiles)
- Drone reuse

**Scriptable Objects (Unity)**
- Drone modules (data)
- Behaviors (configuration)
- Production recipes
- Resources

### Optimization

#### Performance

**Target Metrics:**
- 60 FPS with 100 drones
- 30 FPS with 500 drones
- WebGL support (browser constraints)

**Optimization Methods:**
- **Level of Detail (LOD)**: Simplify drones far from camera
- **Culling**: Don't process objects off-screen
- **Spatial Hashing**: Fast nearest object search
- **Job System**: Multithreading for physics (Unity Jobs)
- **Batch Rendering**: Combine identical sprites
- **Object Pooling**: Object reuse

#### Build Size

**Goal**: <50 MB for WebGL (fast loading)

**Methods:**
- Code stripping (already configured in project)
- Texture atlas (all sprites in one file)
- Audio compression
- JavaScript minification

---

## Metrics and Balance

### KPI (Key Performance Indicators)

**Engagement:**
- Average session length: >30 minutes
- Retention:
  - Day 1: >60%
  - Day 7: >30%
  - Day 30: >10%

**Progression:**
- Average time to mission 10: 2-3 hours
- % players unlocking sandbox: >70%
- % players getting all 3 stars on mission: 40-50%

**Difficulty:**
- First mission: 100% completion rate
- Mission 10: 80% completion rate
- Mission 20: 50% completion rate (expert level)

### Balance

**Early Game (first 30 minutes):**
- Fast results (first drone in 2 minutes)
- Simple goals (collect 100 iron)
- Minimal micromanagement

**Mid Game (30-120 minutes):**
- Increased logistics complexity (3-5 production stages)
- Strategy choice (many simple drones or few complex ones?)
- First optimization challenges

**Late Game (120+ minutes):**
- Megaprojects (hundreds of drones, dozens of bases)
- Rare resources (0.5% chance to find)
- Extreme optimization (every % efficiency matters)

**Difficulty Curve:**
```
Difficulty
    ^
    |                    ╱───── Late game
    |                 ╱╱
    |              ╱╱ Mid game
    |          ╱╱╱
    |      ╱╱╱ Early game
    |  ╱╱╱
    |╱
    └────────────────────────────> Time
```

---

## Development Roadmap

### MVP (Minimum Viable Product) - 2-3 months

**Goal**: Prove concept, basic gameplay

- ✅ Movement physics with inertia
- ✅ Drone constructor (3 module types: core, engine, mining)
- ✅ 1 resource type (iron)
- ✅ 1 behavior (Mining Drone)
- ✅ Procedural asteroid field generation
- ✅ Basic UI (drone selection, view characteristics)
- ✅ 3 tutorial missions
- ✅ Save/load

### Alpha - 1-2 months

**Goal**: Complete game loop

- ➕ 5 module types (fuel, power, cargo)
- ➕ 3 resource types (iron, copper, ice)
- ➕ 3 behaviors (Mining, Hauler, Scout)
- ➕ Production structures (smelter, storage)
- ➕ Production chain (ore → metal → modules → drone)
- ➕ 10 missions
- ➕ Sandbox mode (basic)
- ➕ Visual effects (particles, animations)

### Beta - 1-2 months

**Goal**: Polish, content

- ➕ All module types (15+)
- ➕ All behaviors (7)
- ➕ All resources (12)
- ➕ Technology tree
- ➕ 20 missions (complete campaign)
- ➕ Dynamic events (flares, meteors)
- ➕ Gravity mechanics
- ➕ Sound and music
- ➕ UI/UX polish

### Release - 1 month

**Goal**: Release 1.0

- ➕ Balance
- ➕ Optimization (60 FPS with 100+ drones)
- ➕ Tutorial (complete)
- ➕ Localization (English + Russian minimum)
- ➕ Testing
- ➕ Marketing materials

### Post-Release (DLC / Updates)

**Possible Additions:**
- Survival mode (hostile drones?)
- Multiplayer (cooperative mining)
- Level editor
- New zones (gas giants, Kuiper belt)
- Advanced technologies (teleportation, warp drives)

---

## Target Audience and Monetization

### Target Audience

**Primary:**
- Age: 16-35 years
- Gender: 70% male, 30% female
- Interests: Strategy, puzzles, simulators
- Experience: From casual to hardcore

**Reference Games (what they play):**
- Factorio
- Satisfactory
- Dyson Sphere Program
- Opus Magnum
- SpaceChem
- FTL

**Player Portrait:**
> "I like solving logical puzzles and optimizing systems. I value games where you can think several steps ahead. I want to see the results of my efforts - how my factory works by itself"

### Monetization Model

**Option 1: Premium (recommended)**
- One-time purchase ($10-15)
- All content included
- No additional payments
- **Pros**: Loyal audience, good reviews
- **Cons**: One-time revenue

**Option 2: Free-to-Play + Cosmetics**
- Free game
- Paid skins for drones/structures
- Optional premium subscription (faster research)
- **Pros**: More players
- **Cons**: Risk of community backlash

**Option 3: Free Demo + Full Game**
- First 5 missions free
- Full version ($10)
- **Pros**: Low barrier to entry, trial conversion
- **Cons**: Difficulty balancing demo

**Recommendation**: Option 3 for WebGL (easy to try), Option 1 for Steam

---

## Risks and Challenges

### Technical Risks

**1. WebGL Performance**
- **Problem**: Browsers have limitations (memory, CPU)
- **Solution**: Aggressive optimization, drone limit, LOD

**2. Physics with Many Objects**
- **Problem**: 100+ drones can lag
- **Solution**: Spatial partitioning, approximations far from camera

**3. AI Pathfinding Complexity**
- **Problem**: A* accounting for inertia is computationally expensive
- **Solution**: Path caching, simplified physics for prediction

### Design Risks

**1. High Barrier to Entry**
- **Problem**: Space physics may be too complex
- **Solution**: Tutorial, assistants (autopilot), forgiving gameplay on easy levels

**2. Insufficient Variety**
- **Problem**: Game may become boring after 10 hours
- **Solution**: Procedural generation, events, sufficient content

**3. Production Chain Balance**
- **Problem**: Too easy or too hard
- **Solution**: Active testing, metrics, post-release patches

### Marketing Risks

**1. Competition with Major Games**
- **Problem**: Factorio already occupies niche
- **Solution**: Unique space physics, more accessible gameplay

**2. Store Visibility**
- **Problem**: Hard to stand out among indies
- **Solution**: Quality trailer, demo, community engagement

---

## Success Criteria

### Minimal Success
- 1,000 downloads in first month
- Rating >4.0/5
- 10+ positive reviews

### Medium Success
- 10,000 downloads in first 3 months
- Rating >4.5/5
- Mentions in indie reviews

### Major Success
- 100,000 downloads in first year
- Rating >4.7/5
- Active community (fan content, mods)
- Indie award nominations

---

## Conclusion

**Asteroid Foundry** is an ambitious project combining Factorio's depth with Into the Breach's accessibility, spiced with unique space physics. The key to success is balancing complexity and accessibility.

**Next Steps:**
1. ✅ Create design document (complete)
2. ⏭️ Prototype movement physics (1 week)
3. ⏭️ Create drone constructor (1 week)
4. ⏭️ First mission "First Flight" (1 week)
5. ⏭️ Focus group testing (get feedback)

**Development Philosophy:**
> "Iterate fast, test often, listen to players"

---

*This document is living. It will be updated throughout development, testing, and receiving player feedback.*

**Version 1.0** | Created: 2025-11-26 | Next Update: After MVP
