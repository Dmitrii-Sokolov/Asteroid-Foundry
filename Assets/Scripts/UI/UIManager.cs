using UnityEngine;

namespace AsteroidFoundry.UI
{
    /// <summary>
    /// Manages all UI panels and screens
    /// Coordinates UI state and transitions
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        public static UIManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                Debug.LogWarning("[UIManager] Duplicate instance destroyed");
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeUI();
        }

        #endregion

        #region UI Components

        [Header("UI Panels")]
        [SerializeField] private HUDController _hudController;

        // Other UI components will be added as we implement them:
        // [SerializeField] private DroneDesignerUI _droneDesigner;
        // [SerializeField] private BehaviorEditorUI _behaviorEditor;
        // [SerializeField] private MissionUI _missionUI;
        // [SerializeField] private PauseMenuUI _pauseMenu;

        public HUDController HUD => _hudController;

        #endregion

        #region Initialization

        private void InitializeUI()
        {
            Debug.Log("[UIManager] Initializing UI...");

            // Subscribe to game events
            Core.EventBus.Subscribe<Core.GameStateChangedEvent>(OnGameStateChanged);
            Core.EventBus.Subscribe<Core.GamePausedEvent>(OnGamePaused);
            Core.EventBus.Subscribe<Core.GameResumedEvent>(OnGameResumed);

            // Initialize UI components
            if (_hudController != null)
            {
                _hudController.Initialize();
            }

            Debug.Log("[UIManager] UI initialized");
        }

        void OnDestroy()
        {
            // Unsubscribe from events
            Core.EventBus.Unsubscribe<Core.GameStateChangedEvent>(OnGameStateChanged);
            Core.EventBus.Unsubscribe<Core.GamePausedEvent>(OnGamePaused);
            Core.EventBus.Unsubscribe<Core.GameResumedEvent>(OnGameResumed);
        }

        #endregion

        #region Event Handlers

        private void OnGameStateChanged(Core.GameStateChangedEvent evt)
        {
            Debug.Log($"[UIManager] Game state changed: {evt.previousState} -> {evt.newState}");

            switch (evt.newState)
            {
                case Core.GameManager.GameState.MainMenu:
                    ShowMainMenu();
                    break;

                case Core.GameManager.GameState.Playing:
                    ShowGameplayUI();
                    break;

                case Core.GameManager.GameState.Paused:
                    ShowPauseMenu();
                    break;

                case Core.GameManager.GameState.GameOver:
                    ShowGameOverScreen();
                    break;
            }
        }

        private void OnGamePaused(Core.GamePausedEvent evt)
        {
            // Additional pause UI handling if needed
        }

        private void OnGameResumed(Core.GameResumedEvent evt)
        {
            // Additional resume UI handling if needed
        }

        #endregion

        #region UI State Management

        private void ShowMainMenu()
        {
            // Hide all gameplay UI
            if (_hudController != null)
                _hudController.Hide();

            // Show main menu (will implement later)
            Debug.Log("[UIManager] Showing main menu");
        }

        private void ShowGameplayUI()
        {
            // Show HUD
            if (_hudController != null)
                _hudController.Show();

            // Hide menus
            Debug.Log("[UIManager] Showing gameplay UI");
        }

        private void ShowPauseMenu()
        {
            // Show pause overlay
            Debug.Log("[UIManager] Showing pause menu");
        }

        private void ShowGameOverScreen()
        {
            // Show game over screen
            Debug.Log("[UIManager] Showing game over screen");
        }

        #endregion

        #region Public API

        public void TogglePanel(GameObject panel)
        {
            if (panel != null)
            {
                panel.SetActive(!panel.activeSelf);
            }
        }

        public void ShowPanel(GameObject panel)
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        public void HidePanel(GameObject panel)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        #endregion
    }
}
