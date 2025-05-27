using UnityEngine;

namespace AsteroidFoundry
{
    public class ThrustControl : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D Rigidbody;

        [Tooltip("Good values are from 0.1 to 100")]
        [SerializeField] private float AngularAcceleration;
        [SerializeField] private float LinearAcceleration;
        [SerializeField] private float MaxSpeed;
        [SerializeField] private float AngularTolerance;

        public Vector2 TargetDirection { get; set; }

        private void FixedUpdate()
        {
            var targetVelocity = TargetDirection * MaxSpeed;
            var currentVelocity = Rigidbody.linearVelocity;
            var velocityDifference = targetVelocity - currentVelocity;
            var targetDirection = velocityDifference.sqrMagnitude > Mathf.Epsilon
                ? velocityDifference
                : TargetDirection;


            if (targetDirection != Vector2.zero)
            {
                var currentRotationDegrees = Rigidbody.rotation;
                var targetRotationDegrees = Vector2.SignedAngle(Vector2.right, targetDirection);
                var angularDifference = Mathf.DeltaAngle(currentRotationDegrees, targetRotationDegrees) * Mathf.Deg2Rad;
                var angularSpeed = Rigidbody.angularVelocity * Mathf.Deg2Rad;
                var power = GetTorquePower(angularDifference, angularSpeed, AngularAcceleration);
                Rigidbody.AddTorque(power * AngularAcceleration * Rigidbody.inertia);


                if (Mathf.Abs(angularDifference) <= AngularTolerance)
                {
                    var targetAcceleration = velocityDifference / Time.fixedDeltaTime;
                    var accelerationPower = Vector2.ClampMagnitude(targetAcceleration, LinearAcceleration) / LinearAcceleration;
                    Rigidbody.AddForce(accelerationPower * LinearAcceleration);
                }
            }

            Debug.DrawLine(Rigidbody.position, Rigidbody.position + Rigidbody.linearVelocity, Color.red);
        }

        private static float GetTorquePower(
            float angularDifference,
            float angularSpeed,
            float maxAngularAcceleration)
        {
            var targetV = 0f;
            var accelerationStep = maxAngularAcceleration * Time.fixedDeltaTime;

            if (Mathf.Abs(angularDifference) > Mathf.Epsilon)
            {
                var targetVApproximate = Mathf.Sqrt(2f * Mathf.Abs(angularDifference) * maxAngularAcceleration);
                var stepsToTargetAngle = Mathf.Ceil(targetVApproximate / accelerationStep);
                targetV = angularDifference / stepsToTargetAngle / Time.fixedDeltaTime + 0.5f * Mathf.Sign(angularDifference) * (stepsToTargetAngle - 1) * accelerationStep;
            }

            var torquePower = (targetV - angularSpeed) / accelerationStep;

            return Mathf.Clamp(torquePower, -1f, 1f);
        }
    }
}
