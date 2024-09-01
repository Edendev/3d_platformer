using Game.SceneManagement;
using Game.Systems;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{   
    public class GameManager : SystemGroup
    {
        public static GameManager Instance
        {
            get
            {
                if (instance == null) return new GameManager();
                return instance;
            }
        }

        private static GameManager instance = null;
        public uint CurrentLevelId { get; private set; } = 0;

        // Persistent
        private GameSOContainerBehaviour gameSOContainer = null;
        private SettingsSystem settingsSystem = null;
        
        // Non-persistent
        private GameUIBehaviour gameUI = null;
        private GameKeyElementsBehaviour gameKeyElements = null;
        private UpdateSystem updateSystem = null;

        public GameManager() { 
            instance = this;

            // Get persistent references from scene
            gameSOContainer = GameObject.FindObjectOfType<GameSOContainerBehaviour>();

            if (gameSOContainer == null)
            {
                throw new Exception($"{nameof(GameSOContainerBehaviour)} is missing in the boot scene.");
            }

            // Create settings system
            settingsSystem = new SettingsSystem(gameSOContainer.LevelSettingsSO);

            Initialize();

            ScenesManager.Instance.onSceneStartsLoading += HandleOnSceneStartsLoading;
            ScenesManager.Instance.onSceneFinishLoading += HandleOnSceneFinishLoading;
        } 
        
        private void Initialize()
        {
            // Get scene references
            gameUI = GameObject.FindObjectOfType<GameUIBehaviour>();

            if (gameUI == null)
            {
                throw new Exception($"{nameof(GameUIBehaviour)} is missing in the scene.");
            }

            gameKeyElements = GameObject.FindObjectOfType<GameKeyElementsBehaviour>();

            if (gameKeyElements == null)
            {
                throw new Exception($"{nameof(GameKeyElementsBehaviour)} is missing in the scene.");
            }
            
            // Create all non-persistent systems
            ExecutorSystem executorSystem;
            LevelTimerSystem levelTimerSystem;
            CameraControlSystem cameraControlSystem;
            PlayerSystem playerSystem;
            TransformablesSystem transformablesSystem;
            GameStateSystem gameStateSystem;
            CollectiblesSystem collectiblesSystem;
            InteractablesSystem interactablesSystem;

            AddSystem(updateSystem = new UpdateSystem(settingsSystem));
            AddSystem(executorSystem = new ExecutorSystem(updateSystem));
            AddSystem(levelTimerSystem = new LevelTimerSystem(updateSystem, gameUI));
            AddSystem(collectiblesSystem = new CollectiblesSystem(gameUI));
            AddSystem(interactablesSystem = new InteractablesSystem(gameKeyElements.Interactables));
            AddSystem(cameraControlSystem = new CameraControlSystem(gameSOContainer.GameSettingsSO, updateSystem, settingsSystem));
            AddSystem(playerSystem = new PlayerSystem(updateSystem, gameSOContainer.GameSettingsSO, cameraControlSystem.CameraTransform));
            AddSystem(transformablesSystem = new TransformablesSystem(gameKeyElements.Transformables, updateSystem));
            AddSystem(gameStateSystem = new GameStateSystem(settingsSystem, updateSystem, executorSystem, levelTimerSystem, transformablesSystem, interactablesSystem, cameraControlSystem, playerSystem, gameSOContainer.GameSettingsSO, gameUI, gameKeyElements.LevelCompletedTrigger));
        }

        private void HandleOnSceneStartsLoading(int index)
        {
            Dispose();
            gameKeyElements = null;
            gameUI = null;
            updateSystem = null;
        }

        private void HandleOnSceneFinishLoading(int index)
        {
            if (!settingsSystem.TryGetLevelIdFromSceneBuildIndex(index, out uint levelId))
            {
#if UNITY_EDITOR
                Debug.LogError($"Could not find level for scene build index {index} after loading complete.");
#endif
                return;
            }
            CurrentLevelId = levelId;
            Initialize();
        }

        public void FrameUpdate(float deltaTime)
        {
            updateSystem.FrameUpdate(deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            updateSystem.FixedUpdate(deltaTime);
        }
    }
}