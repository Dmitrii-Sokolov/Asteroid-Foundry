using UnityEngine;

namespace AsteroidFoundry.Core
{
    /// <summary>
    /// Core game manager - handles game lifecycle, state management, and system coordination
    /// Singleton pattern ensures only one instance exists
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance { get; private set; }

        void Awake()
        {
            // Singleton pattern - ensure only one instance exists
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                Debug.LogWarning("[GameManager] Duplicate instance destroyed");
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGame();
        }

        #endregion

        #region State Management

        public enum GameState
        {
            Initializing,
            MainMenu,
            Playing,
            Paused,
            GameOver
        }

        public enum GameMode
        {
            None,
            Mission,
            Sandbox
        }

        [Header("Game State")]
        [SerializeField] private GameState _currentState = GameState.Initializing;
        [SerializeField] private GameMode _currentMode = GameMode.None;

        public GameState CurrentState => _currentState;
        public GameMode CurrentMode => _currentMode;

        #endregion

        #region System References

        [Header("Core Systems")]
        public TimeManager TimeManager { get; private set; }

        // Other system references will be added as we implement them:
        // public DroneManager DroneManager { get; private set; }
        // public ResourceManager ResourceManager { get; private set; }
        // public PhysicsManager PhysicsManager { get; private set; }
        // public MissionManager MissionManager { get; private set; }
        // public UIManager UIManager { get; private set; }

        #endregion

        #region Initialization

        private void InitializeGame()
        {
            Debug.Log("[GameManager] Initializing game...");

            // 1. Initialize infrastructure
            InitializeInfrastructure();

            // 2. Initialize core systems (will be added later)
            InitializeSystems();

            // 3. Set initial state
            ChangeState(GameState.MainMenu);

            Debug.Log("[GameManager] Game initialized successfully");
        }

        private void InitializeInfrastructure()
        {
            // EventBus initialization (will implement next)
            // Object pool initialization (Phase 3+)
        }

        private void InitializeSystems()
        {
            // Find or create TimeManager
            TimeManager = FindObjectOfType<TimeManager>();
            if (TimeManager == null)
            {
                GameObject timeManagerObj = new GameObject("TimeManager");
                timeManagerObj.transform.SetParent(transform);
                TimeManager = timeManagerObj.AddComponent<TimeManager>();
            }

            // Other systems will be initialized here as we create them
        }

        #endregion

        #region State Changes

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState)
                return;

            Debug.Log($"[GameManager] State change: {_currentState} -> {newState}");

            // Exit current state
            OnStateExit(_currentState);

            // Change state
            GameState previousState = _currentState;
            _currentState = newState;

            // Enter new state
            OnStateEnter(newState, previousState);

            // Publish state change event
            EventBus.Publish(new GameStateChangedEvent { newState = newState, previousState = previousState });
        }

        private void OnStateExit(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    // Cleanup playing state if needed
                    break;
                case GameState.Paused:
                    // Cleanup pause state
                    break;
            }
        }

        private void OnStateEnter(GameState state, GameState previousState)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    // Show main menu UI (will implement later)
                    break;

                case GameState.Playing:
                    // Resume game time if coming from pause
                    if (previousState == GameState.Paused)
                    {
                        TimeManager?.ResumeGame();
                    }
                    break;

                case GameState.Paused:
                    // Pause game time
                    TimeManager?.PauseGame();
                    break;

                case GameState.GameOver:
                    // Show game over UI
                    break;
            }
        }

        #endregion

        #region Game Mode Management

        public void StartMission(int missionId)
        {
            Debug.Log($"[GameManager] Starting mission {missionId}");
            _currentMode = GameMode.Mission;
            ChangeState(GameState.Playing);

            // Load mission data and setup (will implement in Phase 7)
            // MissionManager?.LoadMission(missionId);
        }

        public void StartSandbox(int seed = 0)
        {
            Debug.Log($"[GameManager] Starting sandbox mode (seed: {seed})");
            _currentMode = GameMode.Sandbox;
            ChangeState(GameState.Playing);

            // Generate procedural world (will implement in Phase 5)
            // WorldManager?.GenerateWorld(seed);
        }

        public void ReturnToMenu()
        {
            Debug.Log("[GameManager] Returning to main menu");
            _currentMode = GameMode.None;
            ChangeState(GameState.MainMenu);

            // Cleanup game world
            // CleanupGameWorld();
        }

        #endregion

        #region Application Lifecycle

        void OnApplicationQuit()
        {
            Debug.Log("[GameManager] Application quitting");

            // Save game state before quitting (will implement in Phase 4)
            // SaveManager?.SaveGame();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && CurrentState == GameState.Playing)
            {
                // Auto-pause when application loses focus
                ChangeState(GameState.Paused);
            }
        }

        #endregion

        #region Unity Lifecycle

        void Update()
        {
            // Handle input for pause (ESC key)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CurrentState == GameState.Playing)
                {
                    ChangeState(GameState.Paused);
                }
                else if (CurrentState == GameState.Paused)
                {
                    ChangeState(GameState.Playing);
                }
            }
        }

        #endregion

        #region Debug Helpers

#if UNITY_EDITOR
        [ContextMenu("Debug: Print Game State")]
        private void DebugPrintState()
        {
            Debug.Log($"Current State: {CurrentState}, Current Mode: {CurrentMode}");
        }

        [ContextMenu("Debug: Force Pause")]
        private void DebugForcePause()
        {
            ChangeState(GameState.Paused);
        }

        [ContextMenu("Debug: Force Resume")]
        private void DebugForceResume()
        {
            ChangeState(GameState.Playing);
        }
#endif

        #endregion
    }
}
