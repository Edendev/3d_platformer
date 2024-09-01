using Game.Player;
using System.Collections;
using System.Collections.Generic;
using Game.Settings;
using UnityEngine;

namespace Game.Systems
{
    public class SettingsSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private LevelSettingsSO[] levelSettings = new LevelSettingsSO[0];

        public SettingsSystem(LevelSettingsSO[] allLevelSettings)
        {
            levelSettings = new LevelSettingsSO[allLevelSettings.Length];
          
            foreach (LevelSettingsSO levelSettingsSO in allLevelSettings)
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
            levelSettings = null;
        }

        public bool HasLevel(uint levelId)
        {
            return (levelId < levelSettings.Length);
        }

        public Vector3 GetLevelStartPosition(uint levelId)
        {
            if (levelId >= levelSettings.Length) return Vector3.zero;
            return levelSettings[levelId].PlayerStartPosition;
        }

        public Vector3 GetCameraUIPosition(uint levelId)
        {
            if (levelId >= levelSettings.Length) return Vector3.zero;
            return levelSettings[levelId].CameraUIPosition;
        }

        public Vector3 GetCameraUIRotation(uint levelId)
        {
            if (levelId >= levelSettings.Length) return Vector3.zero;
            return levelSettings[levelId].CameraUIRotation;
        }

        public uint GetLevelUpdatablesCapacity(uint levelId)
        {
            if (levelId >= levelSettings.Length) return LevelSettingsSO.DefaultUpdatablesCapacity;
            return levelSettings[levelId].InitialUpdatablesCapacity;
        }

        public int GetLevelSceneBuildIndex(uint levelId)
        {
            if (levelId >= levelSettings.Length) return -1;
            return levelSettings[levelId].SceneBuildIndex;
        }
        public string GetLevelName(uint levelId)
        {
            if (levelId >= levelSettings.Length) return "";
            return levelSettings[levelId].LevelName;
        }

        public bool TryGetLevelIdFromSceneBuildIndex(int index, out uint levelId)
        {
            levelId = 0;
            for(int i = 0; i < levelSettings.Length; i++)
            {
                if (levelSettings[i].SceneBuildIndex == index)
                {
                    levelId = levelSettings[i].LevelID;
                    return true;
                }
            }
            return false;
        }
    }
}
