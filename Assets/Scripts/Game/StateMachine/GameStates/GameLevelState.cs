using Game.PhysicsSystem;
using Game.Systems;
using UnityEngine;

namespace Game.States
{
    public class GameLevelState : GameState
    {
        private readonly SettingsSystem settingsSystem;
        private readonly LevelTimerSystem levelTimerSystem;
        private readonly TriggerEventsAnnouncer levelCompletedTrigger;
        private readonly PlayerSystem playerSystem;
        private readonly TransformablesSystem transformablesSystem;
        private readonly InteractablesSystem interactablesSystem;
        private readonly GameUIBehaviour gameUI;

        public GameLevelState(uint id, string name, StateMachine stateMachine, SettingsSystem settingsSystem, LevelTimerSystem levelTimerSystem, 
            CameraControlSystem camera, PlayerSystem playerSystem, TransformablesSystem transformablesSystem, InteractablesSystem interactablesSystem, TriggerEventsAnnouncer levelCompletedTrigger, GameUIBehaviour gameUI)
            : base(id, name, stateMachine, camera)
        {
            this.settingsSystem = settingsSystem;
            this.levelTimerSystem = levelTimerSystem;
            this.playerSystem = playerSystem;
            this.transformablesSystem = transformablesSystem;
            this.interactablesSystem = interactablesSystem;
            this.levelCompletedTrigger = levelCompletedTrigger;
            this.gameUI = gameUI;
        }

        public override void Enter() {
            base.Enter();
            levelCompletedTrigger.onTriggerEnter += HandleOnLevelCompletedTriggerEnterEvent;
            playerSystem.SubscribeToDeathEvent(HandleOnPlayerDeathEvent);
            playerSystem.SpawnPlayer(settingsSystem.GetLevelStartPosition(GameManager.Instance.CurrentLevelId));
            transformablesSystem.ResetAllTransformables();
            transformablesSystem.Start();
            interactablesSystem.ResetAllInteractables();
            levelTimerSystem.Start();
            gameUI.EnableTimerCountText();
            gameUI.EnableCollectibleCountText();
            camera.SetTarget(playerSystem);
            camera.ChangeState(StateDefinitions.Camera.FollowTarget);
        }

        private void HandleOnPlayerDeathEvent() {
            StateMachine.ChangeState(StateDefinitions.GameState.GameOver);
        }

        private void HandleOnLevelCompletedTriggerEnterEvent(Collider other) {
            StateMachine.ChangeState(StateDefinitions.GameState.LevelCompleted);
        }

        public override void Exit() {
            levelCompletedTrigger.onTriggerEnter -= HandleOnLevelCompletedTriggerEnterEvent;
            playerSystem.UnsubscribeTFromeathEvent(HandleOnPlayerDeathEvent);
            playerSystem.DespawnPlayer();
            transformablesSystem.Stop();
            levelTimerSystem.Stop();
            base.Exit();
        }
    }
}
