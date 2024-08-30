using Game.CameraControl;
using Game.Player;
using Game.CameraControl;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Category/GameSettings", order = 1)]
    public class GameSettingsSO : ScriptableObject
    {
        public PlayerSO PlayerSO => playerSO;
        public CameraSO CameraSO => cameraSO;
        public float GravityConstant => gravityConstant;
        public float MaxGravitySpeed => maxGravitySpeed;
        public Vector3 CameraUIPosition => cameraUIPosition;
        public Vector3 CameraUIRotation => cameraUIRotation;
        public float TimeToRestartLevel => timeToRestartLevel;

        [SerializeField] private PlayerSO playerSO;
        [SerializeField] private CameraSO cameraSO;
        [SerializeField] private float maxGravitySpeed = -4f;
        [SerializeField] private float gravityConstant = -9.81f;
        [SerializeField] private Vector3 cameraUIPosition;
        [SerializeField] private Vector3 cameraUIRotation;
        [SerializeField] private float timeToRestartLevel = 3f;
    }
}

