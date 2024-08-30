using Game.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Systems;

namespace Game.States
{
    public class GameStartState : GameState
    {
        private readonly GameUIBehaviour gameUI;
        public GameStartState(uint id, string name, StateMachine stateMachine, CameraControlSystem camera, GameUIBehaviour gameUI) : base(id, name, stateMachine, camera) { 
            this.gameUI = gameUI;
        }
        public override System.Type GetType() => typeof(GameStartState);

        public override void Enter()
        {
            base.Enter();
            camera.ChangeState(StateDefinitions.Camera.UI);
            gameUI.EnableStartButton();
            gameUI.SubscribeToStartGameButtonClickEvent(OnStartButtonClickEvent);
        }

        private void OnStartButtonClickEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        public override void Exit()
        {
            gameUI.DisableStartButton();
            gameUI.UnsubscribeFromStartGameButtonClickEvent(OnStartButtonClickEvent);
            base.Exit();
        }
    }
}
