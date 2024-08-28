using Game.Player;
using System.Collections;
using System.Collections.Generic;
using Game.Settings;
using UnityEngine;

namespace Game.Systems
{
    public class SettingsSystem : ISystem
    {
        private LevelSettingsSO[] levelSettings = new LevelSettingsSO[0];

        public void Create()
        {
            // Get all level settings
            foreach (LevelSettingsSO levelSettingsSO in GameManager.Instance.GameSOContainer.LevelSettingsSO)
            {
                if (levelSettingsSO == null) continue;
                if (levelSettingsSO.LevelID >= levelSettings.Length)
                {
#if UNITY_EDITOR
                    Debug.Log($"{nameof(SettingsSystem)} found level settings with level ID greater than number of levels.");
                    continue;
#endif
                }
                levelSettings[levelSettingsSO.LevelID] = levelSettingsSO;
            }
        }

        public void Destroy()
        {

        }

        public Vector3 GetLevelStartPosition(uint levelId)
        {
            if (levelId >= levelSettings.Length) return Vector3.zero;
            return levelSettings[levelId].PlayerStartPosition;
        }

        public uint GetLevelUpdatablesCapacity(uint levelId)
        {
            if (levelId >= levelSettings.Length) return LevelSettingsSO.DefaultUpdatablesCapacity;
            return levelSettings[levelId].InitialUpdatablesCapacity;
        }
    }
}
