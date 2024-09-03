using Game.Settings;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Persistent unity container of all scriptable objects.  
    /// </summary>
    public class GameSOContainerBehaviour : MonoBehaviour
    {
        private static GameSOContainerBehaviour instance;
        public GameSettingsSO GameSettingsSO => gameSettings;
        public InputSettingsSO InputSettingsSO => inputSettings;
        public LevelSettingsSO[] LevelSettingsSO => levelSettings;

        [SerializeField] private GameSettingsSO gameSettings;
        [SerializeField] private InputSettingsSO inputSettings;
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
