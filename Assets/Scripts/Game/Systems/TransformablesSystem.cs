using Game.Transformables;
using UnityEngine;
using System.Collections.Generic;
using System;
using Game.Tween;

namespace Game.Systems
{
    public class TransformablesSystem : ISystem
    {
        private int activeTrasnformablesIndex = 0;
        private TransformableBehaviour[] activeTransformables = new TransformableBehaviour[0];

        private readonly int hash;
        private readonly UpdateSystem updateSystem;

        public TransformablesSystem(TransformableBehaviour[] allTransformables, UpdateSystem updateSystem) {
            this.updateSystem = updateSystem;

            hash = this.GetHashCode();

            activeTransformables = new TransformableBehaviour[allTransformables.Length];

            foreach(TransformableBehaviour transformable in allTransformables)
            {
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
            foreach(TransformableBehaviour transformable in activeTransformables) {
                transformable.DoReset();
            }
        }

        private void HandleOnTransformableStartedEvent(TransformableBehaviour transformable) {
            activeTransformables[activeTrasnformablesIndex] = transformable;
            activeTransformables[activeTrasnformablesIndex].ActiveContainerIndex = activeTrasnformablesIndex;
            activeTransformables[activeTrasnformablesIndex].onStopped += HandleOnTransformableStoppedEvent;
            activeTrasnformablesIndex++;
        }

        private void HandleOnTransformableStoppedEvent(int index) {
            activeTransformables[index].onStopped -= HandleOnTransformableStoppedEvent;

            // Move all transformables one up
            int endIndex = activeTrasnformablesIndex >= activeTransformables.Length ? activeTransformables.Length : activeTrasnformablesIndex + 1;
            for (int i = index + 1; i < endIndex; i++)
            {
                activeTransformables[i - 1] = activeTransformables[i];
            }
            activeTransformables[activeTrasnformablesIndex] = null;
            activeTrasnformablesIndex--;
        }

        public void FrameUpdate(float deltaTime) {
            foreach(TransformableBehaviour transformable in activeTransformables) {
                transformable.FrameUpdate(deltaTime);
            }
        }
    }
}

