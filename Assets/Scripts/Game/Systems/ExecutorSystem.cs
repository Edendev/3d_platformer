using System;
using System.Collections.Generic;

namespace Game.Systems
{
    /// <summary>
    /// Contains all game executors and handles any action execution request.
    /// </summary>
    public class ExecutorSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Public;
        
        private readonly int hash;

        // Container for all executors
        private Dictionary<EUpdateTime, GameExecutor> executors = new Dictionary<EUpdateTime, GameExecutor>();

        public ExecutorSystem(UpdateSystem updateSystem)
        {
            SystemHash.TryGetHash(typeof(ExecutorSystem), out hash);
            
            // Create all executors (use same hash since we are passing actions by UpadateTime)
            foreach(EUpdateTime updateTime in Enum.GetValues(typeof(EUpdateTime))) { 
                executors.Add(updateTime, new GameExecutor(updateSystem, updateTime, hash));
            }
        }

        public void Destroy() {
            executors.Clear();
        }

        public void Execute(EUpdateTime exectionTime, Action action, float delay) {
            executors[exectionTime].Execute(action, delay);
        }
    }
}
