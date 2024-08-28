using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class GameStartState : State
    {
        public GameStartState(uint id, string name, StateMachine stateMachine) : base(id, name, stateMachine) { }
        public override System.Type GetType() => typeof(GameStartState);

        public override void Enter()
        {
            base.Enter();
            GameManager.Instance.GameUI.EnableStartButton();
            GameManager.Instance.GameUI.SubscribeToStartGameButtonClickEvent(OnStartButtonClickEvent);
        }

        private void OnStartButtonClickEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        public override void Exit()
        {
            GameManager.Instance.GameUI.DisableStartButton();
            GameManager.Instance.GameUI.UnsubscribeFromStartGameButtonClickEvent(OnStartButtonClickEvent);
            base.Exit();
        }
    }
}
