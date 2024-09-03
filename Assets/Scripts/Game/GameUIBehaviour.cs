using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// Container of all UI references in scene. Provides functionality to enable/disable and modify UI elements on request.
    /// </summary>
    public class GameUIBehaviour : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button restartGameButton;
        [SerializeField] private Button nextLevelGameButton;
        [SerializeField] private TextMeshProUGUI levelTitleText;
        [SerializeField] private TextMeshProUGUI levelCompletedText;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private TextMeshProUGUI timeCounterText;
        [SerializeField] private TextMeshProUGUI collectiblesCounterText;

        private void Awake()
        {
            levelTitleText.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(false);
            restartGameButton.gameObject.SetActive(false);
            nextLevelGameButton.gameObject.SetActive(false);
            levelCompletedText.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            timeCounterText.gameObject.SetActive(false);
            collectiblesCounterText.gameObject.SetActive(false);
        }

        public void EnableLevelTitleText() => levelTitleText.gameObject.SetActive(true);
        public void DisableLevelTitleText() => levelTitleText.gameObject.SetActive(false);
        public void EnableStartButton() => startGameButton.gameObject.SetActive(true);
        public void DisableStartButton() => startGameButton.gameObject.SetActive(false);
        public void EnableRestartButton() => restartGameButton.gameObject.SetActive(true);
        public void DisableRestartButton() => restartGameButton.gameObject.SetActive(false);
        public void EnableNextLevelButton() => nextLevelGameButton.gameObject.SetActive(true);
        public void DisableNextLevelButton() => nextLevelGameButton.gameObject.SetActive(false);
        public void EnableLevelCompletedText() => levelCompletedText.gameObject.SetActive(true);
        public void DisableLevelCompletedText() => levelCompletedText.gameObject.SetActive(false);
        public void EnableGameOverText() => gameOverText.gameObject.SetActive(true);
        public void DisableGameOverText() => gameOverText.gameObject.SetActive(false);
        public void EnableCollectibleCountText() => collectiblesCounterText.gameObject.SetActive(true);
        public void DisableCollectibleCountText() => collectiblesCounterText.gameObject.SetActive(false);
        public void EnableTimerCountText() => timeCounterText.gameObject.SetActive(true);
        public void DisableTimerCountText() => timeCounterText.gameObject.SetActive(false);

        public void SetLevelTitle(string levelName) => levelTitleText.text = $"Level {levelName}";
        public void SetCollectibleCount(int count) => collectiblesCounterText.text = $"Coins: {count}";
        public void SetTimer(int minutes, int seconds) => timeCounterText.text = $"Time: {minutes}:{seconds}";

        public void SubscribeToStartGameButtonClickEvent(UnityAction action) => startGameButton.onClick.AddListener(action);
        public void UnsubscribeFromStartGameButtonClickEvent(UnityAction action) => startGameButton.onClick.RemoveListener(action);
        public void SubscribeToRestartGameButtonClickEvent(UnityAction action) => restartGameButton.onClick.AddListener(action);
        public void UnsubscribeFromRestartGameButtonClickEvent(UnityAction action) => restartGameButton.onClick.RemoveListener(action);
        public void SubscribeToNextLevelGameButtonClickEvent(UnityAction action) => nextLevelGameButton.onClick.AddListener(action);
        public void UnsubscribeFromNextLevelButtonClickEvent(UnityAction action) => nextLevelGameButton.onClick.RemoveListener(action);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (startGameButton == null) {
                Debug.LogError($"{nameof(startGameButton)} is missing!");
            }

            if (restartGameButton == null) {
                Debug.LogError($"{nameof(restartGameButton)} is missing!");
            }
        }
#endif
    }
}
