using UnityEngine;

namespace AsteroidFoundry
{
    public class TransformShipControl : MonoBehaviour
    {
        [SerializeField] private ThrustControl ThrustControl;
        [SerializeField] private Transform Target;

        private void FixedUpdate()
        {
            Vector2 vector = Target.position - ThrustControl.transform.position;
            ThrustControl.TargetDirection = vector.normalized;
        }
    }
}
