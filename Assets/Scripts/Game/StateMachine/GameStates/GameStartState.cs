using Game.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Systems;

namespace Game.States
{
    public class GameStartState : GameState
    {
        private readonly SettingsSystem settingsSystem;
        private readonly GameUIBehaviour gameUI;
        public GameStartState(uint id, string name, StateMachine stateMachine, SettingsSystem settingsSystem, CameraControlSystem camera, GameUIBehaviour gameUI) : base(id, name, stateMachine, camera) { 
            this.settingsSystem = settingsSystem;
            this.gameUI = gameUI;
        }
        public override System.Type GetType() => typeof(GameStartState);

        public override void Enter()
        {
            base.Enter();
            camera.ChangeState(StateDefinitions.Camera.UI);
            gameUI.SetLevelTitle(settingsSystem.GetLevelName(GameManager.Instance.CurrentLevelId));
            gameUI.EnableLevelTitleText();
            gameUI.EnableStartButton();
            gameUI.SubscribeToStartGameButtonClickEvent(OnStartButtonClickEvent);
        }

        private void OnStartButtonClickEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        public override void Exit()
        {
            gameUI.DisableLevelTitleText();
            gameUI.DisableStartButton();
            gameUI.UnsubscribeFromStartGameButtonClickEvent(OnStartButtonClickEvent);
            base.Exit();
        }
    }
}
