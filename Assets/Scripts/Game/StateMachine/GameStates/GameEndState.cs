using Game.CameraControl;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class GameEndState : GameState
    {
        private readonly GameUIBehaviour gameUI;
        public GameEndState(uint id, string name, StateMachine stateMachine, CameraControlSystem camera, GameUIBehaviour gameUI) : base(id, name, stateMachine, camera) { 
            this.gameUI = gameUI;
        }
        public override System.Type GetType() => typeof(GameEndState);
        public override void Enter()
        {
            base.Enter();
            camera.ChangeState(StateDefinitions.Camera.UI);
            gameUI.EnableRestartButton();
            gameUI.SubscribeToRestartGameButtonClickEvent(OnStartButtonClickEvent);
        }

        private void OnStartButtonClickEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        public override void Exit()
        {
            gameUI.DisableRestartButton();
            gameUI.UnsubscribeFromRestartGameButtonClickEvent(OnStartButtonClickEvent);
            base.Exit();
        }
    }
}
