using Game.CameraControl;
using Game.PhysicsSystem;
using Game.Player;
using Game.Systems;
using UnityEngine;

namespace Game.States
{
    public class GameLevelState : GameState
    {
        private readonly SettingsSystem settingsSystem;
        private readonly TriggerEventsAnnouncer restartTrigger;
        private readonly PlayerSystem playerSystem;
        public GameLevelState(uint id, string name, StateMachine stateMachine, SettingsSystem settingsSystem, CameraControlSystem camera, PlayerSystem playerSystem, TriggerEventsAnnouncer restartTrigger) : base(id, name, stateMachine, camera) { 
            this.settingsSystem = settingsSystem;
            this.restartTrigger = restartTrigger;
            this.playerSystem = playerSystem;
        }
        public override System.Type GetType() => typeof(GameLevelState);

        public override void Enter()
        {
            base.Enter();
            restartTrigger.onTriggerEnter += HandleOnTriggerEnterEvent;
            playerSystem.SpawnPlayer(settingsSystem.GetLevelStartPosition(GameManager.Instance.CurrentLevelId));
            camera.SetTarget(playerSystem);
            camera.ChangeState(StateDefinitions.Camera.FollowTarget);
        }

        private void HandleOnTriggerEnterEvent(Collider other)
        {
            // Only the player can enter the trigger
            StateMachine.ChangeState(StateDefinitions.GameState.End);
        }

        public override void Exit()
        {
            restartTrigger.onTriggerEnter -= HandleOnTriggerEnterEvent;
            playerSystem.DespawnPlayer();
            base.Exit();
        }
    }
}
