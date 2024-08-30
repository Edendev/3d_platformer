using Game.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameEntryPointBehaviour : MonoBehaviour
    {
        GameManager gameManager = null;

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
