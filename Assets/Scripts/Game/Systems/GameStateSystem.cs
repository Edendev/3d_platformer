using Game.Settings;
using Game.PhysicsSystem;
using Game.States;

namespace Game.Systems
{
    /// <summary>
    /// Handles the different states of the game. Follows a state machine pattern.
    /// </summary>
    public class GameStateSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private readonly int hash;

        // State machine
        private readonly StateMachine stateMachine;
        private readonly GameStartState gameStartState;
        private readonly GameLevelState gameLevelState;
        private readonly GameLevelCompletedState gameLevelCompleted;
        private readonly GameOverState gameOverState;

        private readonly UpdateSystem updateSystem;

        public GameStateSystem(SettingsSystem settingsSystem, UpdateSystem updateSystem, ExecutorSystem executorSystem, LevelTimerSystem levelTimerSystem, TransformablesSystem transformablesSystem, InteractablesSystem interactablesSystem, 
            CameraControlSystem camera, PlayerSystem playerSystem, GameSettingsSO gameSettingsSO, GameUIBehaviour gameUI, TriggerEventsAnnouncer levelCompletedTrigger)
        {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(GameStateSystem), out hash);

            // Create and initialize state machine
            stateMachine = new StateMachine();
            gameStartState = new GameStartState(0, StateDefinitions.GameState.Start, stateMachine, settingsSystem, camera, gameUI);
            gameLevelState = new GameLevelState(1, StateDefinitions.GameState.Level, stateMachine, settingsSystem, levelTimerSystem, camera, playerSystem, transformablesSystem, interactablesSystem, levelCompletedTrigger, gameUI);
            gameLevelCompleted = new GameLevelCompletedState(2, StateDefinitions.GameState.LevelCompleted, stateMachine, settingsSystem, camera, gameUI);
            gameOverState = new GameOverState(3, StateDefinitions.GameState.GameOver, stateMachine, camera, gameUI, executorSystem, gameSettingsSO.TimeToRestartLevel);

            stateMachine.AddState(gameStartState);
            stateMachine.AddState(gameLevelState);
            stateMachine.AddState(gameLevelCompleted);
            stateMachine.AddState(gameOverState);

            stateMachine.Initialize();

            updateSystem.AddUpdatable(EUpdateTime.FrameUpdate, hash, FrameUpdate);
            updateSystem.AddUpdatable(EUpdateTime.FixUpdate, hash, FixedUpdate);
            updateSystem.AddUpdatable(EUpdateTime.LateUpdate, hash, LateUpdate);
        }

        public void Destroy() {
            stateMachine.Dispose();
            updateSystem?.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
            updateSystem?.RemoveUpdatable(EUpdateTime.FixUpdate, hash);
            updateSystem?.RemoveUpdatable(EUpdateTime.LateUpdate, hash);
        }

        private void FrameUpdate(float deltaTime) {
            stateMachine.Update(deltaTime);
        }

        private void FixedUpdate(float deltaTime) {
            stateMachine.FixedUpdate(deltaTime);
        }

        private void LateUpdate(float deltaTime) {
            stateMachine.LateUpdate(deltaTime);
        }
    }
}
