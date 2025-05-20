using UnityEngine;
using UnityEngine.InputSystem;

namespace AsteroidFoundry
{
    public class PlayerShipControl : MonoBehaviour
    {
        [SerializeField] private ThrustControl ThrustControl;
        [SerializeField] private InputActionReference MoveAction;

        private void Awake()
        {
            MoveAction.action.performed += OnMove;
            MoveAction.action.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            ThrustControl.TargetDirection = context.ReadValue<Vector2>();
        }

        private void OnDestroy()
        {
            MoveAction.action.performed -= OnMove;
        }
    }
}
