using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine.Rendering.VirtualTexturing;

namespace Game.Systems
{
    public abstract class SystemGroup : IDisposable
    {
        private Dictionary<int, ISystem> systems = new Dictionary<int, ISystem>();

        public void AddSystem(ISystem system)
        {
            if (SystemHash.TryGetHash(system.GetType(), out int hash))
            {
                systems.TryAdd(hash, system);
            }
        }

        public void RemoveSystem(ISystem system)
        {
            if (SystemHash.TryGetHash(system.GetType(), out int hash))
            {
                systems.Remove(hash);
            }
        }

        public bool TryGetSystem<T>(out T system)
            where T : ISystem
        {
            system = default(T);
            if (SystemHash.TryGetHash(typeof(T), out int hash))
            {
                if (!systems.TryGetValue(hash, out ISystem lookAtSystem))
                {
                    return false;
                }
                if (lookAtSystem.AccessType != ESystemAccessType.Public)
                {
                    return false;
                }
                system = (T)lookAtSystem;
            }
            return true;
        }

        public virtual void Dispose()
        {   
            foreach (ISystem system in systems.Values)
            {
                system.Destroy();
            }
            systems.Clear();
        }
    }
}

