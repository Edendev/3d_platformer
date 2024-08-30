using Game.CameraControl;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.States
{
    public class GameOverState : GameState
    {
        private readonly GameUIBehaviour gameUI;
        private readonly ExecutorSystem executorSystem;

        private float timeToRestart;
        public GameOverState(uint id, string name, StateMachine stateMachine, CameraControlSystem camera, GameUIBehaviour gameUI, ExecutorSystem executorSystem, float timeToRestart) : base(id, name, stateMachine, camera) { 
            this.gameUI = gameUI;
            this.executorSystem = executorSystem;
            this.timeToRestart = timeToRestart;
        }
        public override System.Type GetType() => typeof(GameOverState);
        public override void Enter()
        {
            base.Enter();
            camera.ChangeState(StateDefinitions.Camera.UI);
            gameUI.EnableGameOverText();
            executorSystem.Execute(EnableRestart, timeToRestart);
        }

        private void OnRestartButtonClickEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        private void EnableRestart()
        {
            gameUI.EnableRestartButton();
            gameUI.SubscribeToRestartGameButtonClickEvent(OnRestartButtonClickEvent);
        }

        public override void Exit()
        {
            gameUI.DisableGameOverText();
            gameUI.DisableRestartButton();
            gameUI.UnsubscribeFromRestartGameButtonClickEvent(OnRestartButtonClickEvent);
            base.Exit();
        }
    }
}
