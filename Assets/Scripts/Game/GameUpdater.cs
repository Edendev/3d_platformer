using Game.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{    
    public class GameUpdater : IDisposable
    {
        private const int ARRAY_CAPACITY_INCREASE = 10;

        private Dictionary<int, int> updatablesIndexes = new Dictionary<int, int>();

        private int currentUpdatablesIndex = 0;
        private Action<float>[] updatables = new Action<float>[0];

        public GameUpdater(SettingsSystem settingsSystem) {
            updatables = new Action<float>[settingsSystem.GetLevelUpdatablesCapacity(GameManager.Instance.CurrentLevelId)];
        }
        
        public void Update(float deltaTime)
        {
            for (int i = 0; i < currentUpdatablesIndex; i++) { 
                updatables[i].Invoke(deltaTime);
            }
        }

        public void AddUpdatable(int hash, Action<float> updatable)
        {
            if (updatablesIndexes.ContainsKey(hash)) return;

            if (currentUpdatablesIndex >= updatables.Length)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Updatables max capacity reached. Reallocation needed");
#endif
                Action<float>[] newUpdatables = new Action<float>[updatables.Length + ARRAY_CAPACITY_INCREASE];
                Array.Copy(updatables, newUpdatables, updatables.Length);
                updatables = newUpdatables;
            }

            updatables[currentUpdatablesIndex] = updatable; 
            updatablesIndexes.TryAdd(hash, currentUpdatablesIndex);
            currentUpdatablesIndex++;
        }

        public void RemoveUpdatable(int hash)
        {
            if (!updatablesIndexes.ContainsKey(hash)) return;

            int endIndex = currentUpdatablesIndex >= updatables.Length ? updatables.Length : currentUpdatablesIndex + 1;
            for (int i = updatablesIndexes[hash] + 1; i < endIndex; i++)
            {
                updatables[i - 1] = updatables[i];
            }

            updatables[currentUpdatablesIndex] = null;
            updatablesIndexes.Remove(hash);
            currentUpdatablesIndex--;
        }

        public void Dispose()
        {

        }
    }
}
