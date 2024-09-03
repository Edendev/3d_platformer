using Game.CameraControl;
using Game.Player;
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
        public float TimeToRestartLevel => timeToRestartLevel;

        [SerializeField] private PlayerSO playerSO;
        [SerializeField] private CameraSO cameraSO;
        [SerializeField] private float maxGravitySpeed = -4f;
        [SerializeField] private float gravityConstant = -9.81f;
        [SerializeField] private float timeToRestartLevel = 3f;
    }
}

