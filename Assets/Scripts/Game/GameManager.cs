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

        public GameManager() { 
            instance = this;

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
            CameraControlSystem cameraControlSystem = new CameraControlSystem(gameSOContainer.GameSettingsSO, updateSystem);
            PlayerSystem playerSystem = new PlayerSystem(gameSOContainer.GameSettingsSO, updateSystem, cameraControlSystem.CameraTransform);
            GameStateSystem gameStateSystem = new GameStateSystem(settingsSystem, updateSystem, cameraControlSystem, playerSystem, gameUI, gameKeyElements.RestartTrigger);

            AddSystem(settingsSystem);
            AddSystem(updateSystem);
            AddSystem(cameraControlSystem);
            AddSystem(playerSystem);
            AddSystem(gameStateSystem);
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

