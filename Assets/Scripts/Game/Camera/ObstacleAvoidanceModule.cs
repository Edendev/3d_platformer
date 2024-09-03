using UnityEngine;

namespace Game.CameraControl
{
    /// <summary>
    /// Creates an avoidance position offset in the Y direction for the target transform to avoid objects in the specified layer mask
    /// </summary>
    public class ObstacleAvoidanceModule
    {
        private const float DISTANCE_THRESHOLD = 0.1f;
        public Vector3 CurrentAvoidanceOffset => currentAvoidanceOffset; // add this offset to the target transform position

        private readonly float avoidanceRadius;
        private readonly float avoidanceSpeed;
        private readonly Transform transform;
        private readonly int layerMask;

        private Collider[] obstacles = new Collider[0];
        private Vector3 currentAvoidanceOffset = Vector3.zero;

        public ObstacleAvoidanceModule(float avoidanceRadius, float avoidanceSpeed, int layerMask, Transform transform) { 
            this.avoidanceRadius = avoidanceRadius;
            this.avoidanceSpeed = avoidanceSpeed;
            this.transform = transform;
            this.layerMask = layerMask;
        }

        public void Update(float deltaTime) {            
            // If any overlapping obstacle, move offset upwards to avoid it 
            obstacles = Physics.OverlapSphere(transform.position, avoidanceRadius, layerMask);
            if (obstacles.Length == 0) {
                // If the avoidance offset is greater than the threshold, try to decrease it
                if (currentAvoidanceOffset.magnitude >= DISTANCE_THRESHOLD) {
                    Vector3 deltaOffset = currentAvoidanceOffset.normalized * avoidanceSpeed * deltaTime;
                    obstacles = Physics.OverlapSphere(transform.position - deltaOffset, avoidanceRadius, layerMask);
                    if (obstacles.Length == 0) {
                        currentAvoidanceOffset -= deltaOffset;
                    }
                }
                return;
            }

            currentAvoidanceOffset = currentAvoidanceOffset + Vector3.up * avoidanceSpeed * deltaTime;
        }
    }
}
