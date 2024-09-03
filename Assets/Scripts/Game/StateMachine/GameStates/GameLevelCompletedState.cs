using Game.SceneManagement;
using Game.Systems;

namespace Game.States
{
    public class GameLevelCompletedState : GameState
    {
        private readonly GameUIBehaviour gameUI;
        private readonly SettingsSystem settingsSystem;

        private int nextSceneBuildIndex = 0;

        public GameLevelCompletedState(uint id, string name, StateMachine stateMachine, SettingsSystem settingsSystem, CameraControlSystem camera, GameUIBehaviour gameUI) : base(id, name, stateMachine, camera)
        {
            this.settingsSystem = settingsSystem;
            this.gameUI = gameUI;
        }

        public override void Enter() {
            base.Enter();
            camera.ChangeState(StateDefinitions.Camera.UI);
            gameUI.EnableLevelCompletedText();
            gameUI.EnableRestartButton();
            gameUI.SubscribeToRestartGameButtonClickEvent(OnRestartButtonClickEvent);
            if (settingsSystem.HasLevel(GameManager.Instance.CurrentLevelId + 1)) {
                gameUI.EnableNextLevelButton();
                gameUI.SubscribeToNextLevelGameButtonClickEvent(OnNextLevelButtonClickEvent);
            }
        }

        private void OnRestartButtonClickEvent() {
            StateMachine.ChangeState(StateDefinitions.GameState.Level);
        }

        private void OnNextLevelButtonClickEvent() {
            nextSceneBuildIndex = settingsSystem.GetLevelSceneBuildIndex(GameManager.Instance.CurrentLevelId + 1);
            if (nextSceneBuildIndex == -1) return;
            ScenesManager.Instance.LoadScene(nextSceneBuildIndex);
        }

        public override void Exit() {
            gameUI.DisableLevelCompletedText();
            gameUI.DisableRestartButton();
            gameUI.DisableNextLevelButton();
            gameUI.UnsubscribeFromRestartGameButtonClickEvent(OnRestartButtonClickEvent);
            gameUI.UnsubscribeFromNextLevelButtonClickEvent(OnNextLevelButtonClickEvent);
            base.Exit();
        }
    }
}
