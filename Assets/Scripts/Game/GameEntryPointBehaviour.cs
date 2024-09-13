using Game.SceneManagement;
using Game.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{   
    /// <summary>
    /// Unity root singleton. 
    /// Initializes the managers and functions as the funnel to update all the game systems.
    /// </summary>
    public class GameEntryPointBehaviour : MonoBehaviour
    {
        private static GameEntryPointBehaviour instance;

        private GameManager gameManager = null;
        private ScenesManager scenesManager = null;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Initialize managers
            scenesManager = ScenesManager.Instance;
            gameManager = GameManager.Instance;

            scenesManager.onSceneStartsLoading += HandleOnSceneStartsLoading;
            scenesManager.onSceneFinishLoading += HandleOnSceneFinishLoading;
        }

        private void HandleOnSceneStartsLoading(int index) {
            enabled = false;
        }

        private void HandleOnSceneFinishLoading(int index) {
            enabled = true;
        }

        private void OnDestroy()
        {
            gameManager?.Dispose();
            if (scenesManager != null)
            {
                scenesManager.onSceneStartsLoading -= HandleOnSceneStartsLoading;
                scenesManager.onSceneFinishLoading -= HandleOnSceneFinishLoading;
            }
        }

        private void Update() {
            gameManager.FrameUpdate(Time.deltaTime);
        }

        private void FixedUpdate() {
            gameManager.FixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            gameManager.LateUpdate(Time.deltaTime);
        }
    }
}
