using Game.Player;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Category/GameSettings", order = 1)]
    public class GameSettingsSO : ScriptableObject
    {
        public PlayerSO PlayerSO => playerSO;

        [SerializeField] private PlayerSO playerSO;
    }
}

