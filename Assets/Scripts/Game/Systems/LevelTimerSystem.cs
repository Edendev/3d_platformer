using UnityEngine;

namespace Game.Systems
{
    /// <summary>
    /// Communicates with the UI to update the level time counter.
    /// </summary>
    public class LevelTimerSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private readonly int hash;

        private readonly UpdateSystem updateSystem;
        private readonly GameUIBehaviour gameUI;

        private float timer;

        public LevelTimerSystem(UpdateSystem updateSystem, GameUIBehaviour gameUI)
        {
            this.updateSystem = updateSystem;
            this.gameUI = gameUI;
            this.timer = Time.time;
            SystemHash.TryGetHash(typeof(LevelTimerSystem), out hash);
        }

        public void Destroy() { }

        public void Start() {
            timer = Time.time;
            updateSystem.AddUpdatable(EUpdateTime.FrameUpdate, hash, Update);
        }

        public void Stop() {
            updateSystem.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
        }

        public void Update(float deltaTime) {
            float elapsed = Time.time - timer;
            if (Time.time - timer >= 1) gameUI.SetTimer(Mathf.FloorToInt(elapsed / 60f), (int)(elapsed % 60));
        }
    }
}
