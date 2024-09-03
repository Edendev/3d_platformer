using Game.PhysicsSystem;
using System;
using UnityEngine;

namespace Game.Tween
{
    /// <summary>
    /// Provides the player the ability to stick to moving grounds, such as platforms.
    /// </summary>
    public class TweenableModule : IDisposable
    {
        private readonly PhysicsModule physics; 
        private readonly Transform transform;

        private Vector3 groundOldPosition;
        
        public TweenableModule(PhysicsModule physics, Transform transform)
        {
            this.physics = physics;
            this.transform = transform;
            physics.onGroundChanged += HandleOnGroundChangedEvent;
        }

        public void Update(float deltaTime) {
            if (!physics.IsGrounded) return;
            transform.position += physics.Ground.position - groundOldPosition;
            groundOldPosition = physics.Ground.position;
        }

        private void HandleOnGroundChangedEvent() {            
            if (physics.Ground == null) return;
            groundOldPosition = physics.Ground.position;
        }
        
        public void Dispose() {
            if (physics != null) physics.onGroundChanged -= HandleOnGroundChangedEvent;
        }
    }
}

