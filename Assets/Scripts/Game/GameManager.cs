using Game.Systems;
using System;
using System.Collections.Generic;
using Game.States;
using UnityEngine;
using Game.CameraControl;
using Game.Settings;

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

        private struct GameLiveState
        {
            public uint currentLevelId;
        }

        public uint CurrentLevelId => gameLiveState.currentLevelId;
        private GameLiveState gameLiveState;

        private readonly GameSOContainerBehaviour gameSOContainer = null;
        private readonly GameUIBehaviour gameUI = null;
        private readonly GameKeyElementsBehaviour gameKeyElements = null;

        private readonly UpdateSystem updateSystem = null;

        private readonly float timer;

        public GameManager() { 
            instance = this;

            // Initialize timer
            timer = Time.time;

            // Create game live state and set initial values
            gameLiveState = new GameLiveState();
            gameLiveState.currentLevelId = 0;

            // Get scene references
            gameSOContainer = GameObject.FindObjectOfType<GameSOContainerBehaviour>();

            if (gameSOContainer == null) {
                throw new Exception($"{nameof(GameSOContainerBehaviour)} is missing in the scene.");
            }

            gameUI = GameObject.FindObjectOfType<GameUIBehaviour>();

            if (gameUI == null) {
                throw new Exception($"{nameof(GameUIBehaviour)} is missing in the scene.");
            }

            gameKeyElements = GameObject.FindObjectOfType<GameKeyElementsBehaviour>();

            if (gameKeyElements == null) {
                throw new Exception($"{nameof(GameKeyElementsBehaviour)} is missing in the scene.");
            }

            // Create all necessary systems
            SettingsSystem settingsSystem = new SettingsSystem(gameSOContainer.LevelSettingsSO);
            updateSystem = new UpdateSystem(settingsSystem);
            ExecutorSystem executorSystem = new ExecutorSystem(updateSystem);
            CameraControlSystem cameraControlSystem = new CameraControlSystem(gameSOContainer.GameSettingsSO, updateSystem);
            PlayerSystem playerSystem = new PlayerSystem(gameSOContainer.GameSettingsSO, updateSystem, cameraControlSystem.CameraTransform);
            TransformablesSystem transformablesSystem = new TransformablesSystem(gameKeyElements.Transformables, updateSystem);
            GameStateSystem gameStateSystem = new GameStateSystem(settingsSystem, updateSystem, executorSystem, transformablesSystem, cameraControlSystem, playerSystem, gameSOContainer.GameSettingsSO, gameUI, gameKeyElements.LevelCompletedTrigger);

            AddSystem(settingsSystem);
            AddSystem(updateSystem);
            AddSystem(executorSystem);   
            AddSystem(cameraControlSystem);
            AddSystem(playerSystem);
            AddSystem(gameStateSystem);
            AddSystem(transformablesSystem);
        }  

        public void FrameUpdate(float deltaTime)
        {
            float elapsed = Time.time - timer;
            if (Time.time - timer >= 1) gameUI.SetTimer(Mathf.FloorToInt(elapsed / 60f), (int)(elapsed % 60));
            updateSystem.FrameUpdate(deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            updateSystem.FixedUpdate(deltaTime);
        }
    }
}

