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

        private static GameManager instance;
        public uint CurrentLevelId { get; private set; } = 0;

        private GameSOContainerBehaviour gameSOContainer = null;
        private GameUIBehaviour gameUI = null;
        private GameKeyElementsBehaviour gameKeyElements = null;

        private UpdateSystem updateSystem = null;

        private GameEntryPointBehaviour entryPointBehaviour = null;

        public GameManager() { 
            instance = this;
            Initialize();
        } 
        
        private void Initialize()
        {
            // Get entry point 
            entryPointBehaviour = GameObject.FindObjectOfType<GameEntryPointBehaviour>();

            if (entryPointBehaviour == null)
            {
                throw new Exception($"{nameof(GameEntryPointBehaviour)} is missing in the scene.");
            }

            // Get scene references
            gameSOContainer = GameObject.FindObjectOfType<GameSOContainerBehaviour>();

            if (gameSOContainer == null)
            {
                throw new Exception($"{nameof(GameSOContainerBehaviour)} is missing in the scene.");
            }

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

            // Create all necessary systems
            SettingsSystem settingsSystem;
            ExecutorSystem executorSystem;
            LevelTimerSystem levelTimerSystem;
            CameraControlSystem cameraControlSystem;
            PlayerSystem playerSystem;
            TransformablesSystem transformablesSystem;
            GameStateSystem gameStateSystem;
            CollectiblesSystem collectiblesSystem;
            InteractablesSystem interactablesSystem;

            AddSystem(settingsSystem = new SettingsSystem(gameSOContainer.LevelSettingsSO));
            AddSystem(updateSystem = new UpdateSystem(settingsSystem));
            AddSystem(executorSystem = new ExecutorSystem(updateSystem));
            AddSystem(levelTimerSystem = new LevelTimerSystem(updateSystem, gameUI));
            AddSystem(collectiblesSystem = new CollectiblesSystem(gameUI));
            AddSystem(interactablesSystem = new InteractablesSystem(gameKeyElements.Interactables));
            AddSystem(cameraControlSystem = new CameraControlSystem(gameSOContainer.GameSettingsSO, updateSystem));
            AddSystem(playerSystem = new PlayerSystem(updateSystem, gameSOContainer.GameSettingsSO, cameraControlSystem.CameraTransform));
            AddSystem(transformablesSystem = new TransformablesSystem(gameKeyElements.Transformables, updateSystem));
            AddSystem(gameStateSystem = new GameStateSystem(settingsSystem, updateSystem, executorSystem, levelTimerSystem, transformablesSystem, interactablesSystem, cameraControlSystem, playerSystem, gameSOContainer.GameSettingsSO, gameUI, gameKeyElements.LevelCompletedTrigger));

            entryPointBehaviour.enabled = true;
        }

        public async void LoadScene(int index)
        {
            Dispose();
            gameKeyElements = null;
            gameSOContainer = null;
            gameUI = null;
            updateSystem = null;
            entryPointBehaviour.enabled = false; 
            await LoadSceneTask(index);
            CurrentLevelId++;
            Initialize();
        }

        private async Task LoadSceneTask(int index)
        {   
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

            while (true)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    break;
                }
            }

            await Task.Delay(100);
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