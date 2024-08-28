using Game.Player;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class GameLevelState : State
    {
        private readonly SettingsSystem settingsSystem;

        public GameLevelState(uint id, string name, StateMachine stateMachine, SettingsSystem settingsSystem) : base(id, name, stateMachine) { 
            this.settingsSystem = settingsSystem;
        }
        public override System.Type GetType() => typeof(GameLevelState);

        public override void Enter()
        {
            base.Enter();

            // Spawn player at start position
            PlayerController player = GameObject.Instantiate<PlayerController>(GameManager.Instance.GameSOContainer.GameSettingsSO.PlayerSO.Player);
            player.transform.position = settingsSystem.GetLevelStartPosition(GameManager.Instance.CurrentLevelId);
        }
    }
}
