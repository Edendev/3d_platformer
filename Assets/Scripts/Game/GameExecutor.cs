using Game.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Handles any request for an action to be executed at a specific time and with a specific delay 
    /// </summary>
    public class GameExecutor : IDisposable
    {
        private struct ExecutionElement
        {
            public float timer;
            public float delay;
            public Action action;
        }

        private readonly int hash;
        private readonly EUpdateTime executionTime;
        private readonly UpdateSystem updateSystem;

        private List<ExecutionElement> executionList = new List<ExecutionElement>();

        public GameExecutor(UpdateSystem updateSystem, EUpdateTime executionTime, int hash) { 
            this.updateSystem = updateSystem;
            this.hash = hash;
            this.executionTime = executionTime;
        }

        public void Update(float deltaTime) {
            // Only update if there are elements
            if (executionList.Count == 0) {
                updateSystem.RemoveUpdatable(executionTime, hash);
                return;
            }

            // Update reversed to be able to remove elements
            for (int i = executionList.Count - 1; i >= 0; i--) {
                if (Time.time - executionList[i].timer >= executionList[i].delay) {
                    executionList[i].action.Invoke();
                    executionList.RemoveAt(i);
                }
            }
        }

        public void Execute(Action action, float delay) {
            executionList.Add(new ExecutionElement { action = action, delay = delay, timer = Time.time });
            updateSystem.AddUpdatable(executionTime, hash, Update);
        }

        public void Dispose() {
            executionList.Clear();
        }
    }
}
