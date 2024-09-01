using Game.Utils;
using UnityEngine;

namespace Game.CameraControl
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Category/Camera", order = 1)]
    public class CameraSO : ScriptableObject
    {
        public GameObject CameraGO => cameraGO;
        public PositionRotation[] TargetOffsets => targetOffsets;
        public float MovementSpeedMultiplier => movementSpeedMultiplier;
        public float RotationSpeed => rotationSpeed;
        public float ObstacleAvoidanceRadius => obstacleAvoidanceRadius;
        public float ObstacleAvoidanceSpeed => obstacleAvoidanceSpeed;

        [SerializeField] private GameObject cameraGO;
        [SerializeField, Tooltip("Offsets relative to the target when in Follow Target State")]
        private PositionRotation[] targetOffsets;
        [SerializeField, Tooltip("Multiplier to the default movement speed based on the distance to the target position")] private float movementSpeedMultiplier;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float obstacleAvoidanceRadius;
        [SerializeField] private float obstacleAvoidanceSpeed;
    }
}
