using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.CameraControl
{
    public class ObstacleAvoidanceModule
    {
        private const float DISTANCE_THRESHOLD = 0.1f;
        public Vector3 CurrentAvoidanceOffset => currentAvoidanceOffset;

        private readonly float avoidanceRadius;
        private readonly float avoidanceSpeed;
        private readonly Transform transform;
        private readonly int layerMask;

        private Collider[] obstacles = new Collider[0];
        private Vector3 currentAvoidanceOffset = Vector3.zero;

        public ObstacleAvoidanceModule(float avoidanceRadius, float avoidanceSpeed, Transform transform) { 
            this.avoidanceRadius = avoidanceRadius;
            this.avoidanceSpeed = avoidanceSpeed;
            this.transform = transform;
            this.layerMask = LayerMask.GetMask("Default");
        }

        public void Update(float deltaTime) {
            obstacles = Physics.OverlapSphere(transform.position, avoidanceRadius, layerMask);
            if (obstacles.Length == 0) {
                if (currentAvoidanceOffset.magnitude >= DISTANCE_THRESHOLD)
                {
                    Vector3 deltaOffset = currentAvoidanceOffset.normalized * avoidanceSpeed * deltaTime;
                    obstacles = Physics.OverlapSphere(transform.position - deltaOffset, avoidanceRadius, layerMask);
                    if (obstacles.Length == 0)
                    {
                        currentAvoidanceOffset -= deltaOffset;
                    }
                }
                return;
            }

            currentAvoidanceOffset = currentAvoidanceOffset + Vector3.up * avoidanceSpeed * deltaTime;
        }
    }
}
