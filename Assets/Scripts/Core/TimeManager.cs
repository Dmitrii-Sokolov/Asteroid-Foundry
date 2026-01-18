using UnityEngine;

namespace AsteroidFoundry.Core
{
    /// <summary>
    /// Manages game time, including pause, time scale (game speed), and time tracking
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        #region Time State

        [Header("Time State")]
        [SerializeField] private bool _isPaused = false;
        [SerializeField] private float _timeScale = 1f;
        [SerializeField] private float _gameTime = 0f; // Total game time (excluding pauses)

        public bool IsPaused => _isPaused;
        public float TimeScale => _timeScale;
        public float GameTime => _gameTime;

        /// <summary>
        /// Time since game started, excluding pauses
        /// </summary>
        public float UnpausedTime => _gameTime;

        /// <summary>
        /// Delta time scaled by time scale, returns 0 when paused
        /// </summary>
        public float DeltaTime => _isPaused ? 0f : Time.deltaTime * _timeScale;

        #endregion

        #region Time Scale Presets

        public const float NORMAL_SPEED = 1f;
        public const float FAST_SPEED = 2f;
        public const float VERY_FAST_SPEED = 4f;
        public const float SLOW_MOTION = 0.5f;

        #endregion

        #region Events

        // Events will be published via EventBus once implemented
        // For now, using UnityEvents as a placeholder

        public event System.Action OnPaused;
        public event System.Action OnResumed;
        public event System.Action<float> OnTimeScaleChanged;

        #endregion

        #region Unity Lifecycle

        void Awake()
        {
            // Initialize with normal time
            SetTimeScale(NORMAL_SPEED);
        }

        void Update()
        {
            if (!_isPaused)
            {
                _gameTime += Time.deltaTime * _timeScale;
            }

            // Debug input for time controls (will move to InputManager later)
            HandleDebugInput();
        }

        #endregion

        #region Pause/Resume

        public void PauseGame()
        {
            if (_isPaused)
                return;

            _isPaused = true;
            Time.timeScale = 0f;

            Debug.Log("[TimeManager] Game paused");
            OnPaused?.Invoke();

            // Publish event
            EventBus.Publish(new GamePausedEvent { gameTime = _gameTime });
        }

        public void ResumeGame()
        {
            if (!_isPaused)
                return;

            _isPaused = false;
            Time.timeScale = _timeScale;

            Debug.Log("[TimeManager] Game resumed");
            OnResumed?.Invoke();

            // Publish event
            EventBus.Publish(new GameResumedEvent { gameTime = _gameTime });
        }

        public void TogglePause()
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        #endregion

        #region Time Scale

        public void SetTimeScale(float newTimeScale)
        {
            newTimeScale = Mathf.Clamp(newTimeScale, 0.1f, 10f); // Reasonable limits

            if (Mathf.Approximately(_timeScale, newTimeScale))
                return;

            float previousScale = _timeScale;
            _timeScale = newTimeScale;

            // Apply to Unity's Time.timeScale (only if not paused)
            if (!_isPaused)
            {
                Time.timeScale = _timeScale;
            }

            Debug.Log($"[TimeManager] Time scale changed: {previousScale:F1}x -> {_timeScale:F1}x");
            OnTimeScaleChanged?.Invoke(_timeScale);

            // Publish event
            EventBus.Publish(new TimeScaleChangedEvent { newScale = newTimeScale, previousScale = previousScale });
        }

        public void SetNormalSpeed() => SetTimeScale(NORMAL_SPEED);
        public void SetFastSpeed() => SetTimeScale(FAST_SPEED);
        public void SetVeryFastSpeed() => SetTimeScale(VERY_FAST_SPEED);
        public void SetSlowMotion() => SetTimeScale(SLOW_MOTION);

        #endregion

        #region Time Queries

        /// <summary>
        /// Get formatted time string (MM:SS)
        /// </summary>
        public string GetFormattedTime()
        {
            int minutes = Mathf.FloorToInt(_gameTime / 60f);
            int seconds = Mathf.FloorToInt(_gameTime % 60f);
            return $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// Get formatted time string with hours (HH:MM:SS)
        /// </summary>
        public string GetFormattedTimeLong()
        {
            int hours = Mathf.FloorToInt(_gameTime / 3600f);
            int minutes = Mathf.FloorToInt((_gameTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(_gameTime % 60f);
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }

        #endregion

        #region Debug Input

        private void HandleDebugInput()
        {
#if UNITY_EDITOR
            // Space - toggle pause
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TogglePause();
            }

            // Number keys - change speed
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetNormalSpeed();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SetFastSpeed();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                SetVeryFastSpeed();
            if (Input.GetKeyDown(KeyCode.Alpha4))
                SetSlowMotion();
#endif
        }

        #endregion

        #region Save/Load Support

        [System.Serializable]
        public struct TimeData
        {
            public float gameTime;
            public float timeScale;
            public bool isPaused;
        }

        public TimeData GetSaveData()
        {
            return new TimeData
            {
                gameTime = _gameTime,
                timeScale = _timeScale,
                isPaused = _isPaused
            };
        }

        public void LoadSaveData(TimeData data)
        {
            _gameTime = data.gameTime;
            SetTimeScale(data.timeScale);

            if (data.isPaused)
                PauseGame();
            else
                ResumeGame();

            Debug.Log($"[TimeManager] Loaded save data: {GetFormattedTimeLong()}, {_timeScale}x speed");
        }

        #endregion

        #region Debug Helpers

#if UNITY_EDITOR
        [ContextMenu("Debug: Print Current Time")]
        private void DebugPrintTime()
        {
            Debug.Log($"Game Time: {GetFormattedTimeLong()}, Time Scale: {_timeScale}x, Paused: {_isPaused}");
        }

        [ContextMenu("Debug: Reset Time")]
        private void DebugResetTime()
        {
            _gameTime = 0f;
            Debug.Log("[TimeManager] Time reset to 0");
        }

        [ContextMenu("Debug: Add 1 Minute")]
        private void DebugAddMinute()
        {
            _gameTime += 60f;
            Debug.Log($"[TimeManager] Added 1 minute. New time: {GetFormattedTimeLong()}");
        }
#endif

        #endregion
    }
}
