using UnityEngine;

namespace AsteroidFoundry
{
    public class ThrustControl : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D Rigidbody;

        [SerializeField] private float Acceleration;
        [SerializeField] private float MaxSpeed;

        public Vector2 TargetDirection { get; set; }

        private void FixedUpdate()
        {
            var targetVelocity = TargetDirection * MaxSpeed;
            var currentVelocity = Rigidbody.linearVelocity;
            var velocityDifference = targetVelocity - currentVelocity;

            if (velocityDifference.magnitude > 0)
            {
                //TODO Use angular forces instead of setting rotation directly
                var forceDirection = Vector2.SignedAngle(Vector2.right, velocityDifference);
                Rigidbody.rotation = forceDirection;

                //TODO Use force instead of setting velocity directly
                var maxSpeedChange = Acceleration * Time.fixedDeltaTime;
                Rigidbody.linearVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, maxSpeedChange);
            }
        }
    }
}
