using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneManagement
{
    /// <summary>
    /// Handles the scene loading process
    /// </summary>
    public class ScenesManager
    {
        public event Action<int> onSceneStartsLoading;
        public event Action<int> onSceneFinishLoading;

        public static ScenesManager Instance
        {
            get
            {
                if (instance == null) return new ScenesManager();
                return instance;
            }
        }

        private static ScenesManager instance = null;

        public ScenesManager() {
            instance = this;
        }

        public async void LoadScene(int index)
        {
            onSceneStartsLoading?.Invoke(index); // perform any necessary pre-load action here
            await LoadSceneTask(index);
            onSceneFinishLoading?.Invoke(index); // perform any necessary post-load action here
        }

        private async Task LoadSceneTask(int index)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

            while (true)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    break;
                }
            }

            await Task.Delay(100); // ensure scene completely loaded
        }
    }
}
