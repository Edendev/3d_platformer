using Game.Player;
using System.Collections;
using System.Collections.Generic;
using Game.Settings;
using UnityEngine;
using Game.PhysicsSystem;
using Game.States;
using static StateDefinitions;
using Game.CameraControl;

namespace Game.Systems
{
    public class GameStateSystem : ISystem
    {
        private readonly StateMachine stateMachine;

        private readonly GameStartState gameStartState;
        private readonly GameLevelState gameLevelState;
        private readonly GameEndState gameEndState;

        private readonly int hash;

        private readonly UpdateSystem updateSystem;

        public GameStateSystem(SettingsSystem settingsSystem, UpdateSystem updateSystem, CameraControlSystem camera, PlayerSystem playerSystem, GameUIBehaviour gameUI, TriggerEventsAnnouncer restartTrigger)
        {
            this.updateSystem = updateSystem;
            hash = this.GetHashCode();

            // Create and initialize state machine
            stateMachine = new StateMachine();
            gameStartState = new GameStartState(0, StateDefinitions.GameState.Start, stateMachine, camera, gameUI);
            gameLevelState = new GameLevelState(1, StateDefinitions.GameState.Level, stateMachine, settingsSystem, camera, playerSystem, restartTrigger);
            gameEndState = new GameEndState(2, StateDefinitions.GameState.End, stateMachine, camera, gameUI);

            stateMachine.AddState(gameStartState);
            stateMachine.AddState(gameLevelState);
            stateMachine.AddState(gameEndState);

            stateMachine.Initialize();

            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash, FrameUpdate);
            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash, FixedUpdate);
        }

        public void Destroy()
        {
            stateMachine.Dispose();
            updateSystem.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
            updateSystem.RemoveUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash);
        }

        private void FrameUpdate(float deltaTime)
        {
            stateMachine.Update(deltaTime);
        }

        private void FixedUpdate(float deltaTime)
        {
            stateMachine.FixedUpdate(deltaTime);
        }
    }
}
