using System.Collections.Generic;
using Game.Player;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "InputSettings", menuName = "Category/InputSettings", order = 1)]
    public class InputSettingsSO : ScriptableObject
    {
        [System.Serializable]
        public struct PlayerActionKey
        {
            public EPlayerAction action;
            public KeyCode key;
        }

        public IReadOnlyList<PlayerActionKey> PlayerActionKeys => playerActionKeys;

        [SerializeField] private List<PlayerActionKey> playerActionKeys;
    }
}
