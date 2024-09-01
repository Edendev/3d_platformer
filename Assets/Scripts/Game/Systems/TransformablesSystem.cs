using Game.Transformables;
using UnityEngine;
using System.Collections.Generic;
using System;
using Game.Tween;

namespace Game.Systems
{
    public class TransformablesSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private int activeTransformablesIndex = 0;
        private TransformableBehaviour[] activeTransformables = new TransformableBehaviour[0];

        private readonly int hash;
        private readonly UpdateSystem updateSystem;

        public TransformablesSystem(TransformableBehaviour[] allTransformables, UpdateSystem updateSystem) {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(TransformablesSystem), out hash);

            activeTransformables = new TransformableBehaviour[allTransformables.Length];

            foreach(TransformableBehaviour transformable in allTransformables)
            {
                transformable.Initialize();
                transformable.onStarted += HandleOnTransformableStartedEvent;
                if (transformable.StartTrigger == TransformableBehaviour.ETransformableStartTrigger.OnEnable) {
                    transformable.TryPlay();
                }
            }
        }

        public void Destroy()
        {
            updateSystem?.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
        }

        public void Start()
        {
            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash, FrameUpdate);
        }

        public void Stop()
        {
            updateSystem.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
        }

        public void ResetAllTransformables()
        {
            for (int i = 0; i < activeTransformablesIndex; i++)
            {
                activeTransformables[i].DoReset();
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

            // Move all transformables one up
            int endIndex = activeTransformablesIndex >= activeTransformables.Length ? activeTransformables.Length : activeTransformablesIndex + 1;
            for (int i = index + 1; i < endIndex; i++)
            {
                activeTransformables[i - 1] = activeTransformables[i];
            }
            activeTransformables[activeTransformablesIndex - 1] = null;
            activeTransformablesIndex--;
        }

        public void FrameUpdate(float deltaTime) {
            for(int i = 0; i < activeTransformablesIndex; i++) {
                activeTransformables[i].FrameUpdate(deltaTime);
            }
        }
    }
}

