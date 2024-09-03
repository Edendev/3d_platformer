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
    /// <summary>
    /// Root C# class managing the game loop
    /// Container of all persistent and non-persistent systems and scene MonoBehaviours.
    /// Handles the update of the game logic.
    /// </summary>
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

        public GameManager() 
        { 
            instance = this;

            // Get persistent references from scene
            gameSOContainer = GameObject.FindObjectOfType<GameSOContainerBehaviour>();

            if (gameSOContainer == null)
            {
                Debug.LogError($"{nameof(GameSOContainerBehaviour)} is missing in the boot scene.");
                return;
            }

            // Create peristent systems system
            settingsSystem = new SettingsSystem(gameSOContainer.LevelSettingsSO, gameSOContainer.InputSettingsSO);

            // Get current level id
            if (!settingsSystem.TryGetLevelIdFromSceneBuildIndex(SceneManager.GetActiveScene().buildIndex, out uint levelId))
            {
                Debug.LogError($"[GameManager] Boot from scene with a build index not corresponding to any level settings.");
                return;
            }

            CurrentLevelId = levelId;

            Initialize();

            ScenesManager.Instance.onSceneStartsLoading += HandleOnSceneStartsLoading;
            ScenesManager.Instance.onSceneFinishLoading += HandleOnSceneFinishLoading;
        } 

        private void Initialize() {
            // Get non-persistent scene references
            gameUI = GameObject.FindObjectOfType<GameUIBehaviour>();

            if (gameUI == null)
            {
                Debug.LogError($"{nameof(GameUIBehaviour)} is missing in the scene.");
                return;
            }

            gameKeyElements = GameObject.FindObjectOfType<GameKeyElementsBehaviour>();

            if (gameKeyElements == null)
            {
                Debug.LogError($"{nameof(GameKeyElementsBehaviour)} is missing in the scene.");
                return;
            }

            // Create and add all non-persistent systems
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
            AddSystem(playerSystem = new PlayerSystem(updateSystem, gameSOContainer.GameSettingsSO, settingsSystem, cameraControlSystem.CameraTransform));
            AddSystem(transformablesSystem = new TransformablesSystem(gameKeyElements.Transformables, updateSystem));
            AddSystem(gameStateSystem = new GameStateSystem(settingsSystem, updateSystem, executorSystem, levelTimerSystem, transformablesSystem, interactablesSystem, cameraControlSystem, playerSystem, gameSOContainer.GameSettingsSO, gameUI, gameKeyElements.LevelCompletedTrigger));
        }

        private void HandleOnSceneStartsLoading(int index) {
            // Dispose all systems and clean up references of all non-persistent objects
            Dispose();
            gameKeyElements = null;
            gameUI = null;
            updateSystem = null;
        }

        private void HandleOnSceneFinishLoading(int index) {
            if (!settingsSystem.TryGetLevelIdFromSceneBuildIndex(index, out uint levelId))
            {
                Debug.LogError($"Could not find level for scene build index {index} after loading complete.");
                return;
            }
            CurrentLevelId = levelId;
            Initialize();
        }

        public void FrameUpdate(float deltaTime) {
            updateSystem.FrameUpdate(deltaTime);
        }

        public void FixedUpdate(float deltaTime) {
            updateSystem.FixedUpdate(deltaTime);
        }

        public void LateUpdate(float deltaTime) { 
            updateSystem.LateUpdate(deltaTime); 
        }
    }
}