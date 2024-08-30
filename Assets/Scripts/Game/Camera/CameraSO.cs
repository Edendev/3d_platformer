using UnityEngine;

namespace Game.CameraControl
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Category/Camera", order = 1)]
    public class CameraSO : ScriptableObject
    {
        public GameObject CameraGO => cameraGO;
        public Vector3 TargetOffset => targetOffset;
        public Vector3 Orientation => orientation;
        public float MovementSpeed => movementSpeed;
        public float RotationSpeed => rotationSpeed;
        public float ObstacleAvoidanceRadius => obstacleAvoidanceRadius;
        public float ObstacleAvoidanceSpeed => obstacleAvoidanceSpeed;

        [SerializeField] private GameObject cameraGO;
        [SerializeField, Tooltip("Offset relative to the target when in Follow Target State")]
        private Vector3 targetOffset;
        [SerializeField] private Vector3 orientation;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float obstacleAvoidanceRadius;
        [SerializeField] private float obstacleAvoidanceSpeed;
    }
}
