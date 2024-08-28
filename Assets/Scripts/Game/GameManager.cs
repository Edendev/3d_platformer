using Game.Systems;
using System;
using System.Collections.Generic;
using Game.States;
using UnityEngine;

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

        public GameSOContainerBehaviour GameSOContainer => gameSOContainer;
        public GameUIBehaviour GameUI => gameUI;
        public GameUpdater GameUpdater => gameUpdater;

        #region StateMachine

        private StateMachine gameStateMachine;
        private GameStartState gameStartState;
        private GameLevelState gameLevelState;
        private GameEndState gameEndState;

        #endregion

        public uint CurrentLevelId => gameLiveState.currentLevelId;
        private GameLiveState gameLiveState;

        private GameSOContainerBehaviour gameSOContainer = null;
        private GameUIBehaviour gameUI = null;
        private readonly GameUpdater gameUpdater;

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

            if (gameUI == null)
            {
                throw new Exception($"{nameof(GameUIBehaviour)} is missing in the scene.");
            }

            // Create and add all necessary systems
            SettingsSystem settings = new SettingsSystem();
            AddSystem(settings);

            // Create updater
            this.gameUpdater = new GameUpdater(settings);

            // Create and initialize state machine
            gameStateMachine = new StateMachine();
            gameStartState = new GameStartState(0, StateDefinitions.GameState.Start, gameStateMachine);
            gameLevelState = new GameLevelState(1, StateDefinitions.GameState.Level, gameStateMachine, settings);
            gameEndState = new GameEndState(2, StateDefinitions.GameState.End, gameStateMachine);

            gameStateMachine.AddState(gameStartState);
            gameStateMachine.AddState(gameLevelState);
            gameStateMachine.AddState(gameEndState);

            gameStateMachine.Initialize();
        }  

        public void FrameUpdate(float deltaTime)
        {
            gameUpdater.FrameUpdate(deltaTime);
        }
    }
}

