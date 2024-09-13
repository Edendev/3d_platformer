using Game.Transformables;

namespace Game.Systems
{
    /// <summary>
    /// Handles the active state and update for all transformables in the scene.
    /// </summary>
    public class TransformablesSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private readonly int hash;

        private readonly UpdateSystem updateSystem;
        private readonly TransformableBehaviour[] allTransformables;

        private int activeTransformablesIndex = 0;
        private TransformableBehaviour[] activeTransformables = new TransformableBehaviour[0];

        public TransformablesSystem(TransformableBehaviour[] allTransformables, UpdateSystem updateSystem) {
            this.updateSystem = updateSystem;
            this.allTransformables = allTransformables;
            SystemHash.TryGetHash(typeof(TransformablesSystem), out hash);

            activeTransformables = new TransformableBehaviour[allTransformables.Length];

            // Initialize all transformables
            foreach(TransformableBehaviour transformable in allTransformables) {
                transformable.Initialize();
                transformable.onStarted += HandleOnTransformableStartedEvent;
                if (transformable.StartTrigger == ETransformableStartTrigger.OnEnable) {
                    transformable.TryPlay();
                }
            }
        }

        public void Destroy() {
            updateSystem?.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
        }

        public void Start() {
            updateSystem.AddUpdatable(EUpdateTime.FrameUpdate, hash, FrameUpdate);
        }

        public void Stop() {
            updateSystem.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
        }

        public void FrameUpdate(float deltaTime) {
            for (int i = 0; i < activeTransformablesIndex; i++) {
                activeTransformables[i].FrameUpdate(deltaTime);
            }
        }

        public void ResetAllTransformables() {
            for (int i = 0; i < allTransformables.Length; i++) {
                allTransformables[i].DoReset();
            }
        }

        private void HandleOnTransformableStartedEvent(TransformableBehaviour transformable) {
            activeTransformables[activeTransformablesIndex] = transformable;
            activeTransformables[activeTransformablesIndex].ActiveContainerIndex = activeTransformablesIndex;
            activeTransformables[activeTransformablesIndex].onStopped += HandleOnTransformableStoppedEvent;
            activeTransformablesIndex++;
        }

        private void HandleOnTransformableStoppedEvent(int index) {
            activeTransformables[index].onStopped -= HandleOnTransformableStoppedEvent;
            activeTransformables[index].ActiveContainerIndex = -1;
            // Swap last one with this
            activeTransformables[index] = activeTransformables[activeTransformablesIndex - 1];
            activeTransformables[activeTransformablesIndex - 1] = null;
            activeTransformablesIndex--;
        }
    }
}

