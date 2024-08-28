using Game.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{    
    public class GameUpdater : IDisposable
    {
        private const int ARRAY_CAPACITY_INCREASE = 10;

        private Dictionary<int, int> updatablesIndexes = new Dictionary<int, int>();

        private int currentUpdatablesIndex = 0;
        private IUpdatable[] updatables = new IUpdatable[0];

        public GameUpdater(SettingsSystem settingsSystem) {
            updatables = new IUpdatable[settingsSystem.GetLevelUpdatablesCapacity(GameManager.Instance.CurrentLevelId)];
        }
        
        public void FrameUpdate(float deltaTime)
        {
            for (int i = 0; i < currentUpdatablesIndex; i++) { 
                updatables[i].FrameUpdate(deltaTime);
            }
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            if (currentUpdatablesIndex >= updatables.Length)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Updatables max capacity reached. Reallocation needed");
#endif
                IUpdatable[] newUpdatables = new IUpdatable[updatables.Length + ARRAY_CAPACITY_INCREASE];
                Array.Copy(updatables, newUpdatables, updatables.Length);
                updatables = newUpdatables;
            }

            updatables[currentUpdatablesIndex] = updatable; 
            updatablesIndexes.TryAdd(updatable.Hash, currentUpdatablesIndex);
            currentUpdatablesIndex++;
        }

        public void RemoveUpdatable(IUpdatable updatable)
        {
            if (!updatablesIndexes.ContainsKey(updatable.Hash)) return;

            for (int i = updatablesIndexes[updatable.Hash] + 1; i < updatables.Length; i++)
            {
                updatables[i - 1] = updatables[i];
            }

            updatables[updatables.Length - 1] = null;
            updatablesIndexes.Remove(updatable.Hash);
            currentUpdatablesIndex--;
        }

        public void Dispose()
        {

        }
    }
}
