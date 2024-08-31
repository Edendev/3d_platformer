using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game.Systems
{
    public class ExecutorSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;
        private struct ExecutionElement
        {
            public float timer;
            public float delay;
            public Action action;
        }

        private readonly int hash;

        private readonly UpdateSystem updateSystem;

        private List<ExecutionElement> executionList = new List<ExecutionElement>();
        private List<ExecutionElement> toRemove = new List<ExecutionElement>();

        public ExecutorSystem(UpdateSystem updateSystem)
        {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(ExecutorSystem), out hash);
        }

        public void Destroy()
        {
            executionList.Clear();
        }

        public void Update(float deltaTime)
        {
            if (executionList.Count == 0) {
                updateSystem.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
                return;
            }

            for(int i = executionList.Count - 1; i >= 0; i--)
            {
                if (Time.time - executionList[i].timer >= executionList[i].delay)
                {
                    executionList[i].action.Invoke();
                    executionList.RemoveAt(i);
                }
            }
        }

        public void Execute(Action action, float delay)
        {
            executionList.Add(new ExecutionElement { action = action, delay = delay, timer = Time.time });
            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash, Update);
        }
    }
}
