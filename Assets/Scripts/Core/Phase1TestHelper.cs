using UnityEngine;

namespace AsteroidFoundry.Core
{
    /// <summary>
    /// Helper script for testing Phase 1 foundation systems
    /// Attach to a GameObject in the scene to test core functionality
    /// </summary>
    public class Phase1TestHelper : MonoBehaviour
    {
        [Header("Test Controls")]
        [SerializeField] private bool _testOnStart = true;
        [SerializeField] private bool _verboseLogging = true;

        void Start()
        {
            if (_testOnStart)
            {
                Debug.Log("=== PHASE 1 FOUNDATION TEST ===");
                TestAllSystems();
            }
        }

        void Update()
        {
            // Test controls
            if (Input.GetKeyDown(KeyCode.T))
            {
                TestAllSystems();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TestEventBus();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                TestGameManager();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                TestTimeManager();
            }
        }

        #region System Tests

        [ContextMenu("Test All Systems")]
        public void TestAllSystems()
        {
            Debug.Log("[Phase1Test] Testing all systems...");

            bool allPassed = true;

            allPassed &= TestGameManager();
            allPassed &= TestTimeManager();
            allPassed &= TestEventBus();
            allPassed &= TestCameraController();
            allPassed &= TestUIManager();

            if (allPassed)
            {
                Debug.Log("<color=green>[Phase1Test] ✓ ALL TESTS PASSED</color>");
            }
            else
            {
                Debug.LogError("[Phase1Test] ✗ SOME TESTS FAILED");
            }
        }

        private bool TestGameManager()
        {
            Log("[Test] Testing GameManager...");

            if (GameManager.Instance == null)
            {
                LogError("GameManager.Instance is null!");
                return false;
            }

            Log($"  - Instance exists: ✓");
            Log($"  - Current State: {GameManager.Instance.CurrentState}");
            Log($"  - Current Mode: {GameManager.Instance.CurrentMode}");

            // Test state change
            var initialState = GameManager.Instance.CurrentState;
            GameManager.Instance.ChangeState(GameManager.GameState.Playing);

            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            {
                LogError("State change failed!");
                return false;
            }

            Log($"  - State change: ✓");
            GameManager.Instance.ChangeState(initialState);

            LogSuccess("GameManager test passed");
            return true;
        }

        private bool TestTimeManager()
        {
            Log("[Test] Testing TimeManager...");

            if (GameManager.Instance == null || GameManager.Instance.TimeManager == null)
            {
                LogError("TimeManager not found!");
                return false;
            }

            var timeManager = GameManager.Instance.TimeManager;

            Log($"  - Instance exists: ✓");
            Log($"  - Game Time: {timeManager.GameTime:F2}s");
            Log($"  - Time Scale: {timeManager.TimeScale}x");
            Log($"  - Is Paused: {timeManager.IsPaused}");

            // Test pause/resume
            bool wasPaused = timeManager.IsPaused;
            timeManager.PauseGame();

            if (!timeManager.IsPaused)
            {
                LogError("Pause failed!");
                return false;
            }

            Log($"  - Pause: ✓");

            timeManager.ResumeGame();

            if (timeManager.IsPaused)
            {
                LogError("Resume failed!");
                return false;
            }

            Log($"  - Resume: ✓");

            // Restore original state
            if (wasPaused)
                timeManager.PauseGame();

            LogSuccess("TimeManager test passed");
            return true;
        }

        private bool TestEventBus()
        {
            Log("[Test] Testing EventBus...");

            // Test subscription and publishing
            bool eventReceived = false;

            System.Action<TestEvent> handler = (evt) =>
            {
                eventReceived = true;
                Log($"  - Event received: {evt.testMessage}");
            };

            EventBus.Subscribe<TestEvent>(handler);
            Log($"  - Subscribed to TestEvent: ✓");

            EventBus.Publish(new TestEvent { testMessage = "Hello from EventBus!" });

            if (!eventReceived)
            {
                LogError("Event was not received!");
                EventBus.Unsubscribe<TestEvent>(handler);
                return false;
            }

            Log($"  - Event published and received: ✓");

            EventBus.Unsubscribe<TestEvent>(handler);
            Log($"  - Unsubscribed: ✓");

            // Verify unsubscribe worked
            eventReceived = false;
            EventBus.Publish(new TestEvent { testMessage = "Should not be received" });

            if (eventReceived)
            {
                LogError("Event was received after unsubscribe!");
                return false;
            }

            Log($"  - Unsubscribe verified: ✓");

            LogSuccess("EventBus test passed");
            return true;
        }

        private bool TestCameraController()
        {
            Log("[Test] Testing CameraController...");

            var cameraController = FindObjectOfType<Input.CameraController>();

            if (cameraController == null)
            {
                LogWarning("CameraController not found in scene (optional)");
                return true; // Not a failure, just optional
            }

            Log($"  - Instance exists: ✓");
            Log($"  - Position: {cameraController.transform.position}");

            // Test mouse world position
            var mouseWorldPos = cameraController.GetMouseWorldPosition();
            Log($"  - Mouse world position: {mouseWorldPos}");
            Log($"  - GetMouseWorldPosition: ✓");

            LogSuccess("CameraController test passed");
            return true;
        }

        private bool TestUIManager()
        {
            Log("[Test] Testing UIManager...");

            var uiManager = UI.UIManager.Instance;

            if (uiManager == null)
            {
                LogWarning("UIManager not found in scene (optional)");
                return true; // Not a failure, just optional
            }

            Log($"  - Instance exists: ✓");

            if (uiManager.HUD != null)
            {
                Log($"  - HUD exists: ✓");
            }

            LogSuccess("UIManager test passed");
            return true;
        }

        #endregion

        #region Test Event

        private struct TestEvent
        {
            public string testMessage;
        }

        #endregion

        #region Logging Helpers

        private void Log(string message)
        {
            if (_verboseLogging)
                Debug.Log(message);
        }

        private void LogSuccess(string message)
        {
            Debug.Log($"<color=green>{message}</color>");
        }

        private void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        private void LogError(string message)
        {
            Debug.LogError(message);
        }

        #endregion

        #region Debug GUI

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 400));
            GUILayout.Label("=== PHASE 1 TEST HELPER ===");
            GUILayout.Space(10);

            if (GUILayout.Button("Run All Tests (T)"))
            {
                TestAllSystems();
            }

            GUILayout.Space(10);
            GUILayout.Label("Individual Tests:");

            if (GUILayout.Button("Test GameManager (G)"))
            {
                TestGameManager();
            }

            if (GUILayout.Button("Test TimeManager (M)"))
            {
                TestTimeManager();
            }

            if (GUILayout.Button("Test EventBus (E)"))
            {
                TestEventBus();
            }

            GUILayout.Space(10);
            GUILayout.Label("Controls:");
            GUILayout.Label("WASD - Move camera");
            GUILayout.Label("Mouse Wheel - Zoom");
            GUILayout.Label("ESC - Pause/Resume");
            GUILayout.Label("Space - Pause/Resume (TimeManager)");
            GUILayout.Label("1-4 - Change time scale");

            GUILayout.EndArea();
        }

        #endregion
    }
}
