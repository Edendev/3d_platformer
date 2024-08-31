using Game.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{   
    public class GameEntryPointBehaviour : MonoBehaviour
    {
        private static GameEntryPointBehaviour instance;
        private GameManager gameManager = null;

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
            // Initialize game manager
            gameManager = GameManager.Instance;
        }

        private void OnDestroy()
        {
            gameManager?.Dispose();
        }

        private void Update()
        {
            gameManager.FrameUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            gameManager.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}
