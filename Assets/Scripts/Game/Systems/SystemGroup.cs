using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public abstract class SystemGroup : IDisposable
    {
        private List<ISystem> systems = new List<ISystem>();

        public void AddSystem(ISystem system)
        {
            systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            systems.Remove(system);
        }

        public virtual void Dispose()
        {
            foreach (ISystem system in systems)
            {
                system.Destroy();
            }
        }
    }
}

