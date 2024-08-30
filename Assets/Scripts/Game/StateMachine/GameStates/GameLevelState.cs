using Game.PhysicsSystem;
using Game.Systems;
using UnityEngine;

namespace Game.States
{
    public class GameLevelState : GameState
    {
        private readonly SettingsSystem settingsSystem;
        private readonly TriggerEventsAnnouncer levelCompletedTrigger;
        private readonly PlayerSystem playerSystem;
        private readonly TransformablesSystem transformablesSystem;

        public GameLevelState(uint id, string name, StateMachine stateMachine, SettingsSystem settingsSystem, CameraControlSystem camera, PlayerSystem playerSystem, TransformablesSystem transformablesSystem, TriggerEventsAnnouncer levelCompletedTrigger)
            : base(id, name, stateMachine, camera)
        {
            this.settingsSystem = settingsSystem;
            this.playerSystem = playerSystem;
            this.transformablesSystem = transformablesSystem;
            this.levelCompletedTrigger = levelCompletedTrigger;
        }
        public override System.Type GetType() => typeof(GameLevelState);

        public override void Enter()
        {
            base.Enter();
            levelCompletedTrigger.onTriggerEnter += HandleOnLevelCompletedTriggerEnterEvent;
            playerSystem.SubscribeToDeathEvent(HandleOnPlayerDeathEvent);
            playerSystem.SpawnPlayer(settingsSystem.GetLevelStartPosition(GameManager.Instance.CurrentLevelId));
            transformablesSystem.ResetAllTransformables();
            transformablesSystem.Start();
            camera.SetTarget(playerSystem);
            camera.ChangeState(StateDefinitions.Camera.FollowTarget);
        }

        private void HandleOnPlayerDeathEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.GameOver);
        }

        private void HandleOnLevelCompletedTriggerEnterEvent(Collider other)
        {
            StateMachine.ChangeState(StateDefinitions.GameState.LevelCompleted);
        }

        public override void Exit()
        {
            levelCompletedTrigger.onTriggerEnter -= HandleOnLevelCompletedTriggerEnterEvent;
            playerSystem.UnsubscribeTFromeathEvent(HandleOnPlayerDeathEvent);
            playerSystem.DespawnPlayer();
            transformablesSystem.Stop();
            base.Exit();
        }
    }
}
