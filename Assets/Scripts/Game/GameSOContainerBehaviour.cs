using Game.Settings;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Game
{
    public class GameSOContainerBehaviour : MonoBehaviour
    {
        private static GameSOContainerBehaviour instance;
        public GameSettingsSO GameSettingsSO => gameSettings;
        public LevelSettingsSO[] LevelSettingsSO => levelSettings;

        [SerializeField] private GameSettingsSO gameSettings;
        [SerializeField] private LevelSettingsSO[] levelSettings;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameSettings == null) {
                Debug.LogError($"{nameof(gameSettings)} is missing!");
            }

            if (levelSettings == null || levelSettings.Length == 0) {
                Debug.LogError($"{nameof(levelSettings)} is missing or there are no level settings!");
            }
        }
#endif
    }
}
