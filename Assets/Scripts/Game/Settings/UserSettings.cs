
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

namespace Game.Settings
{
    public class UserSettings
    {
        public IEnumerable<KeyValuePair<EPlayerAction, KeyCode>> ActionKeys => actionKeys;

        private Dictionary<EPlayerAction, KeyCode> actionKeys = new Dictionary<EPlayerAction, KeyCode>();
        public UserSettings(InputSettingsSO inputSettingsSO) {
            foreach(InputSettingsSO.PlayerActionKey playerActionKey in inputSettingsSO.PlayerActionKeys) { 
                actionKeys.TryAdd(playerActionKey.action, playerActionKey.key);
            }
        }

        public bool TryGetActionKey(EPlayerAction action, out KeyCode key) {
            key = KeyCode.None;
            return actionKeys.TryGetValue(action, out key);
        }
    }
}
