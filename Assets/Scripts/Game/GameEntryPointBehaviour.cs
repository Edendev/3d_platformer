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

        private GameInstance gameInstance = null;

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
            // Create game instance
            gameInstance = new GameInstance();

            gameInstance.sceneLoadingStartedEvent += HandleSceneLoadingStartedEvent;
            gameInstance.sceneLoadingFinishedEvent += HandleSceneLoadingFinishedEvent;
        }

        private void HandleSceneLoadingStartedEvent() {
            enabled = false;
        }

        private void HandleSceneLoadingFinishedEvent() {
            enabled = true;
        }

        private void OnDestroy()
        {
            if (gameInstance != null)
            {
                gameInstance.sceneLoadingStartedEvent -= HandleSceneLoadingStartedEvent;
                gameInstance.sceneLoadingFinishedEvent -= HandleSceneLoadingFinishedEvent;
                gameInstance.Destroy();
            }
        }

        private void Update() {
            gameInstance.FrameUpdate(Time.deltaTime);
        }

        private void FixedUpdate() {
            gameInstance.FixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            gameInstance.LateUpdate(Time.deltaTime);
        }
    }
}
