using System;
using System.Collections.Generic;

namespace Game.Systems
{
    /// <summary>
    /// Container of a group of systems. Has functionality to add/remove and access systems.
    /// </summary>
    public abstract class SystemGroup : IDisposable
    {
        // Systems by hash
        private Dictionary<int, ISystem> systems = new Dictionary<int, ISystem>();

        public void AddSystem(ISystem system) {
            if (SystemHash.TryGetHash(system.GetType(), out int hash)) {
                systems.TryAdd(hash, system);
            }
        }

        public void RemoveSystem(ISystem system) {
            if (SystemHash.TryGetHash(system.GetType(), out int hash)) {
                systems.Remove(hash);
            }
        }

        public bool TryGetSystem<T>(out T system)
            where T : ISystem
        {
            system = default(T);
            if (SystemHash.TryGetHash(typeof(T), out int hash)) {
                if (!systems.TryGetValue(hash, out ISystem lookAtSystem)) {
                    return false;
                }
                if (lookAtSystem.AccessType != ESystemAccessType.Public) {
                    return false;
                }
                system = (T)lookAtSystem;
            }
            return true;
        }

        public virtual void Dispose() {   
            foreach (ISystem system in systems.Values) {
                system.Destroy();
            }
            systems.Clear();
        }
    }
}

