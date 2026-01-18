using UnityEngine;

namespace AsteroidFoundry.Input
{
    /// <summary>
    /// Controls camera movement (WASD/arrows) and zoom (mouse wheel)
    /// Designed for top-down 2D view
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        #region Components

        private Camera _camera;

        #endregion

        #region Movement Settings

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _fastMoveMultiplier = 2f;
        [SerializeField] private bool _enableEdgeScrolling = false;
        [SerializeField] private float _edgeScrollMargin = 20f;

        [Header("Zoom")]
        [SerializeField] private float _zoomSpeed = 2f;
        [SerializeField] private float _minZoom = 2f;   // Orthographic size
        [SerializeField] private float _maxZoom = 50f;
        [SerializeField] private float _zoomSmoothTime = 0.1f;

        [Header("Bounds (Optional)")]
        [SerializeField] private bool _enableBounds = false;
        [SerializeField] private Vector2 _minBounds = new Vector2(-100, -100);
        [SerializeField] private Vector2 _maxBounds = new Vector2(100, 100);

        #endregion

        #region State

        private float _currentZoom;
        private float _targetZoom;
        private float _zoomVelocity;

        private Vector3 _velocity = Vector3.zero;

        #endregion

        #region Unity Lifecycle

        void Awake()
        {
            _camera = GetComponent<Camera>();

            // Ensure camera is orthographic for 2D
            if (!_camera.orthographic)
            {
                Debug.LogWarning("[CameraController] Camera is not orthographic. Setting to orthographic.");
                _camera.orthographic = true;
            }

            // Initialize zoom
            _currentZoom = _camera.orthographicSize;
            _targetZoom = _currentZoom;
        }

        void Update()
        {
            // Don't move camera if game is paused
            if (Core.GameManager.Instance != null &&
                Core.GameManager.Instance.CurrentState == Core.GameManager.GameState.Paused)
            {
                return;
            }

            HandleMovementInput();
            HandleZoomInput();
            ApplyZoom();
        }

        #endregion

        #region Movement

        private void HandleMovementInput()
        {
            Vector3 moveDirection = Vector3.zero;

            // WASD / Arrow keys
            if (UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow))
                moveDirection.y += 1f;
            if (UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow))
                moveDirection.y -= 1f;
            if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow))
                moveDirection.x -= 1f;
            if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow))
                moveDirection.x += 1f;

            // Edge scrolling (optional)
            if (_enableEdgeScrolling)
            {
                Vector3 mousePos = UnityEngine.Input.mousePosition;

                if (mousePos.x < _edgeScrollMargin)
                    moveDirection.x -= 1f;
                else if (mousePos.x > Screen.width - _edgeScrollMargin)
                    moveDirection.x += 1f;

                if (mousePos.y < _edgeScrollMargin)
                    moveDirection.y -= 1f;
                else if (mousePos.y > Screen.height - _edgeScrollMargin)
                    moveDirection.y += 1f;
            }

            // Normalize diagonal movement
            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            // Apply speed multiplier if shift is held
            float currentSpeed = _moveSpeed;
            if (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift))
            {
                currentSpeed *= _fastMoveMultiplier;
            }

            // Move camera
            if (moveDirection != Vector3.zero)
            {
                Vector3 newPosition = transform.position + moveDirection * currentSpeed * Time.deltaTime;

                // Apply bounds if enabled
                if (_enableBounds)
                {
                    newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
                    newPosition.y = Mathf.Clamp(newPosition.y, _minBounds.y, _maxBounds.y);
                }

                transform.position = newPosition;
            }
        }

        #endregion

        #region Zoom

        private void HandleZoomInput()
        {
            // Mouse wheel zoom
            float scrollDelta = UnityEngine.Input.mouseScrollDelta.y;

            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                _targetZoom -= scrollDelta * _zoomSpeed;
                _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
            }
        }

        private void ApplyZoom()
        {
            // Smooth zoom
            _currentZoom = Mathf.SmoothDamp(_currentZoom, _targetZoom, ref _zoomVelocity, _zoomSmoothTime);
            _camera.orthographicSize = _currentZoom;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Move camera to a specific position
        /// </summary>
        public void MoveTo(Vector3 position, float duration = 0f)
        {
            if (duration <= 0f)
            {
                transform.position = new Vector3(position.x, position.y, transform.position.z);
            }
            else
            {
                // Smooth movement (could use coroutine or DOTween)
                StopAllCoroutines();
                StartCoroutine(MoveToCoroutine(position, duration));
            }
        }

        /// <summary>
        /// Set zoom level
        /// </summary>
        public void SetZoom(float zoom, bool instant = false)
        {
            _targetZoom = Mathf.Clamp(zoom, _minZoom, _maxZoom);

            if (instant)
            {
                _currentZoom = _targetZoom;
                _camera.orthographicSize = _currentZoom;
            }
        }

        /// <summary>
        /// Focus on a specific point
        /// </summary>
        public void FocusOn(Vector3 worldPosition, float zoom = 0f)
        {
            MoveTo(worldPosition);

            if (zoom > 0f)
            {
                SetZoom(zoom);
            }
        }

        /// <summary>
        /// Get world position from screen point
        /// </summary>
        public Vector3 ScreenToWorldPoint(Vector3 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }

        /// <summary>
        /// Get world position from mouse position
        /// </summary>
        public Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = UnityEngine.Input.mousePosition;
            mousePos.z = Mathf.Abs(_camera.transform.position.z);
            return _camera.ScreenToWorldPoint(mousePos);
        }

        #endregion

        #region Coroutines

        private System.Collections.IEnumerator MoveToCoroutine(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Smooth ease-out
                t = 1f - Mathf.Pow(1f - t, 3f);

                Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
                newPosition.z = startPosition.z; // Keep z constant
                transform.position = newPosition;

                yield return null;
            }

            Vector3 finalPosition = targetPosition;
            finalPosition.z = startPosition.z;
            transform.position = finalPosition;
        }

        #endregion

        #region Bounds Management

        public void SetBounds(Vector2 min, Vector2 max, bool enable = true)
        {
            _minBounds = min;
            _maxBounds = max;
            _enableBounds = enable;
        }

        public void DisableBounds()
        {
            _enableBounds = false;
        }

        #endregion

        #region Debug Helpers

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (_enableBounds)
            {
                // Draw bounds
                Gizmos.color = Color.yellow;
                Vector3 center = new Vector3(
                    (_minBounds.x + _maxBounds.x) / 2f,
                    (_minBounds.y + _maxBounds.y) / 2f,
                    0f
                );
                Vector3 size = new Vector3(
                    _maxBounds.x - _minBounds.x,
                    _maxBounds.y - _minBounds.y,
                    0f
                );
                Gizmos.DrawWireCube(center, size);
            }
        }

        [ContextMenu("Debug: Reset Camera Position")]
        private void DebugResetPosition()
        {
            transform.position = new Vector3(0, 0, -10);
            SetZoom(10f, true);
        }

        [ContextMenu("Debug: Enable Edge Scrolling")]
        private void DebugEnableEdgeScrolling()
        {
            _enableEdgeScrolling = true;
            Debug.Log("[CameraController] Edge scrolling enabled");
        }
#endif

        #endregion
    }
}
