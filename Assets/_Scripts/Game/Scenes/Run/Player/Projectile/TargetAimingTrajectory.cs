using UnityEngine;
namespace Game.Run
{
    public class TargetAimingTrajectory : ProjectileTrajectory
    {
        public float amplitude = 1.0f; // Adjust the amplitude of the sine wave
        public float frequency = 1.0f; // Adjust the frequency of the sine wave
        public float speed = 5.0f; // Adjust the speed of the arrow

        Transform _target;

        // Set the target for the arrow to follow
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public override void UpdateTrajectory()
        {
            if (_target == null)
            {
                return;
            }

            // Calculate the position along the sine wave
            float yOffset = amplitude * Mathf.Sin(Time.time * frequency);

            // Calculate the direction toward the target
            Vector3 direction = (_target.position - transform.position).normalized;

            // Update the position of the arrow
            transform.position += direction * speed * Time.deltaTime;

            // Apply the sine wave to the Y position
            transform.position += Vector3.up * yOffset * Time.deltaTime;
        }
    }
}
}