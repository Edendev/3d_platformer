using System.Collections.Generic;
using System;

namespace Game.Systems
{
     /// <summary>
     /// Handles the different updates (frame update, fix update and late update) for all objects requiring an update.
     /// Any object that requires and update should subscribe to this system.
     /// </summary>
    public class UpdateSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Public;

        // Container for all updaters in the game
        private readonly Dictionary<EUpdateTime, GameUpdater> updaters = new Dictionary<EUpdateTime, GameUpdater>();

        public UpdateSystem(SettingsSystem settingsSystem)
        {
            // Create all updaters
            foreach (EUpdateTime time in Enum.GetValues(typeof(EUpdateTime))) {
                this.updaters.Add(time, new GameUpdater(settingsSystem));
            }
        }

        public void Destroy() {
            foreach(GameUpdater updater in updaters.Values) {
                updater.Dispose();
            }
        }

        public void AddUpdatable(EUpdateTime time, int hash, Action<float> updatable) {
            updaters[time].AddUpdatable(hash, updatable);
        }

        public void RemoveUpdatable(EUpdateTime time, int hash) {
            updaters[time].RemoveUpdatable(hash);
        }

        public void FrameUpdate(float deltaTime) {
            updaters[EUpdateTime.FrameUpdate].Update(deltaTime);
        }

        public void FixedUpdate(float deltaTime) {
            updaters[EUpdateTime.FixUpdate].Update(deltaTime);
        }

        public void LateUpdate(float deltaTime) {
            updaters[EUpdateTime.LateUpdate].Update(deltaTime);
        }
    }
}
