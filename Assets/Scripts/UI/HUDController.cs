using UnityEngine;
using TMPro;

namespace AsteroidFoundry.UI
{
    /// <summary>
    /// Controls the main HUD (Heads-Up Display)
    /// Shows resources, drone count, time, etc.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        #region UI Elements

        [Header("HUD Elements")]
        [SerializeField] private GameObject _hudRoot;
        [SerializeField] private TextMeshProUGUI _gameTimeText;
        [SerializeField] private TextMeshProUGUI _droneCountText;
        [SerializeField] private TextMeshProUGUI _fpsText;
        [SerializeField] private GameObject _pausedIndicator;

        #endregion

        #region Settings

        [Header("Settings")]
        [SerializeField] private bool _showFPS = true;
        [SerializeField] private float _fpsUpdateInterval = 0.5f;

        #endregion

        #region State

        private bool _isVisible = true;
        private float _fpsTimer = 0f;
        private int _frameCount = 0;

        #endregion

        #region Initialization

        public void Initialize()
        {
            Debug.Log("[HUDController] Initializing HUD");

            // Subscribe to events
            Core.EventBus.Subscribe<Core.GamePausedEvent>(OnGamePaused);
            Core.EventBus.Subscribe<Core.GameResumedEvent>(OnGameResumed);

            // Initial state
            if (_pausedIndicator != null)
                _pausedIndicator.SetActive(false);

            UpdateUI();
        }

        void OnDestroy()
        {
            // Unsubscribe from events
            Core.EventBus.Unsubscribe<Core.GamePausedEvent>(OnGamePaused);
            Core.EventBus.Unsubscribe<Core.GameResumedEvent>(OnGameResumed);
        }

        #endregion

        #region Unity Lifecycle

        void Update()
        {
            if (!_isVisible)
                return;

            UpdateUI();
            UpdateFPS();
        }

        #endregion

        #region UI Updates

        private void UpdateUI()
        {
            // Update game time
            if (_gameTimeText != null && Core.GameManager.Instance != null)
            {
                var timeManager = Core.GameManager.Instance.TimeManager;
                if (timeManager != null)
                {
                    _gameTimeText.text = $"Time: {timeManager.GetFormattedTime()}";
                }
            }

            // Update drone count (placeholder for now)
            if (_droneCountText != null)
            {
                // Will update when DroneManager is implemented
                _droneCountText.text = "Drones: 0 / 0";
            }
        }

        private void UpdateFPS()
        {
            if (!_showFPS || _fpsText == null)
                return;

            _frameCount++;
            _fpsTimer += Time.unscaledDeltaTime;

            if (_fpsTimer >= _fpsUpdateInterval)
            {
                float fps = _frameCount / _fpsTimer;
                _fpsText.text = $"FPS: {fps:F0}";

                _frameCount = 0;
                _fpsTimer = 0f;
            }
        }

        #endregion

        #region Event Handlers

        private void OnGamePaused(Core.GamePausedEvent evt)
        {
            if (_pausedIndicator != null)
                _pausedIndicator.SetActive(true);
        }

        private void OnGameResumed(Core.GameResumedEvent evt)
        {
            if (_pausedIndicator != null)
                _pausedIndicator.SetActive(false);
        }

        #endregion

        #region Public API

        public void Show()
        {
            _isVisible = true;
            if (_hudRoot != null)
                _hudRoot.SetActive(true);
        }

        public void Hide()
        {
            _isVisible = false;
            if (_hudRoot != null)
                _hudRoot.SetActive(false);
        }

        public void ToggleFPS()
        {
            _showFPS = !_showFPS;
            if (_fpsText != null)
                _fpsText.gameObject.SetActive(_showFPS);
        }

        #endregion
    }
}
