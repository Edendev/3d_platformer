using Game.Systems;

namespace Game.States
{
    public class GameLevelCompletedState : GameState
    {
        private readonly GameUIBehaviour gameUI;
        public GameLevelCompletedState(uint id, string name, StateMachine stateMachine, CameraControlSystem camera, GameUIBehaviour gameUI) : base(id, name, stateMachine, camera)
        {
            this.gameUI = gameUI;
        }
        public override System.Type GetType() => typeof(GameLevelCompletedState);
        public override void Enter()
        {
            base.Enter();
            camera.ChangeState(StateDefinitions.Camera.UI);
            gameUI.EnableLevelCompletedText();
            gameUI.EnableRestartButton();
            gameUI.SubscribeToRestartGameButtonClickEvent(OnStartButtonClickEvent);
        }

        private void OnStartButtonClickEvent()
        {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        public override void Exit()
        {
            gameUI.DisableLevelCompletedText();
            gameUI.DisableRestartButton();
            gameUI.UnsubscribeFromRestartGameButtonClickEvent(OnStartButtonClickEvent);
            base.Exit();
        }
    }
}
