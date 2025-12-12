using Game.SceneManagement;
using Game.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{   
    /// <summary>
    /// Root C# class
    /// Container of all persistent and non-persistent systems and scene MonoBehaviours.
    /// Handles the update of the game logic.
    /// </summary>
    public class GameInstance
    {
        public uint CurrentLevelId { get; private set; } = 0;

        public event System.Action sceneLoadingStartedEvent;
        public event System.Action sceneLoadingFinishedEvent;

        private SystemGroup persistentSystems = null;
        private SystemGroup nonPersistentSystems = null;

        // Public access
        public bool TryGetPersistentSystem<T>(out T system) where T : ISystem => persistentSystems.TryGetSystem<T>(out system);
        public bool TryGetSystem<T>(out T system) where T : ISystem => nonPersistentSystems.TryGetSystem<T>(out system); // non persistent systems access

        // Persistent
        private GameSOContainerBehaviour gameSOContainer = null;
        private SettingsSystem settingsSystem = null;
        private ScenesTransitionSystem scenesTransitionSystem = null;
        
        // Non-persistent
        private GameUIBehaviour gameUI = null;
        private GameKeyElementsBehaviour gameKeyElements = null;
        private UpdateSystem updateSystem = null;

        public GameInstance() 
        { 
            // Get persistent references from scene
            gameSOContainer = GameObject.FindObjectOfType<GameSOContainerBehaviour>();

            if (gameSOContainer == null)
            {
                Debug.LogError($"{nameof(GameSOContainerBehaviour)} is missing in the boot scene.");
                return;
            }

            // Create peristent systems system
            persistentSystems = new SystemGroup();
            settingsSystem = new SettingsSystem(gameSOContainer.LevelSettingsSO, gameSOContainer.InputSettingsSO);
            scenesTransitionSystem = new ScenesTransitionSystem();

            // Get current level id
            if (!settingsSystem.TryGetLevelIdFromSceneBuildIndex(SceneManager.GetActiveScene().buildIndex, out uint levelId))
            {
                Debug.LogError($"[GameManager] Boot from scene with a build index not corresponding to any level settings.");
                return;
            }

            CurrentLevelId = levelId;

            scenesTransitionSystem.onSceneStartsLoading += HandleOnSceneStartsLoading;
            scenesTransitionSystem.onSceneFinishLoading += HandleOnSceneFinishLoading;

            Initialize();
        } 

        private void Initialize() {
            // Get non-persistent scene references
            gameUI = GameObject.FindObjectOfType<GameUIBehaviour>();

            if (gameUI == null)
            {
                Debug.LogError($"{nameof(GameUIBehaviour)} is missing in the scene.");
                return;
            }

            // Update UI controls text
            gameUI.SetControls(settingsSystem.ActionKeys);

            gameKeyElements = GameObject.FindObjectOfType<GameKeyElementsBehaviour>();

            if (gameKeyElements == null)
            {
                Debug.LogError($"{nameof(GameKeyElementsBehaviour)} is missing in the scene.");
                return;
            }

            nonPersistentSystems = new SystemGroup();

            // Create and add all non-persistent systems
            ExecutorSystem executorSystem;
            LevelTimerSystem levelTimerSystem;
            CameraControlSystem cameraControlSystem;
            PlayerSystem playerSystem;
            TransformablesSystem transformablesSystem;
            GameStateSystem gameStateSystem;
            CollectiblesSystem collectiblesSystem;
            InteractablesSystem interactablesSystem;

            nonPersistentSystems.AddSystem(updateSystem = new UpdateSystem(this, settingsSystem));
            nonPersistentSystems.AddSystem(executorSystem = new ExecutorSystem(updateSystem));
            nonPersistentSystems.AddSystem(levelTimerSystem = new LevelTimerSystem(updateSystem, gameUI));
            nonPersistentSystems.AddSystem(collectiblesSystem = new CollectiblesSystem(gameUI));
            nonPersistentSystems.AddSystem(interactablesSystem = new InteractablesSystem(this, gameKeyElements.Interactables));
            nonPersistentSystems.AddSystem(cameraControlSystem = new CameraControlSystem(this, gameSOContainer.GameSettingsSO, updateSystem, settingsSystem));
            nonPersistentSystems.AddSystem(playerSystem = new PlayerSystem(updateSystem, gameSOContainer.GameSettingsSO, settingsSystem, cameraControlSystem.CameraTransform));
            nonPersistentSystems.AddSystem(transformablesSystem = new TransformablesSystem(gameKeyElements.Transformables, updateSystem));
            nonPersistentSystems.AddSystem(gameStateSystem = new GameStateSystem(this, scenesTransitionSystem ,settingsSystem, updateSystem, executorSystem, levelTimerSystem, transformablesSystem, interactablesSystem, cameraControlSystem, playerSystem, gameSOContainer.GameSettingsSO, gameUI, gameKeyElements.LevelCompletedTrigger));
        }

        private void HandleOnSceneStartsLoading(int index)
        {
            // Announce
            sceneLoadingStartedEvent?.Invoke();
            // Dispose all non-persistent systems and clean up references of all non-persistent objects
            nonPersistentSystems.Dispose();
            gameKeyElements = null;
            gameUI = null;
            updateSystem = null;
        }

        private void HandleOnSceneFinishLoading(int index)
        {
            // Announce
            sceneLoadingFinishedEvent?.Invoke();
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
        public void Destroy()
        {
            // Unsubscribe to game transition event
            if (scenesTransitionSystem != null)
            {
                scenesTransitionSystem.onSceneStartsLoading -= HandleOnSceneStartsLoading;
                scenesTransitionSystem.onSceneFinishLoading -= HandleOnSceneFinishLoading;
            }

            // Dispose all systems
            persistentSystems?.Dispose();
            nonPersistentSystems?.Dispose();
        }
    }
}