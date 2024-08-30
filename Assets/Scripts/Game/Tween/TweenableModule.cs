using Game.PhysicsSystem;
using Game.Utiles;
using System;
using UnityEngine;

namespace Game.Tween
{
    public class TweenableModule : IDisposable
    {
        private readonly PhysicsModule physics; 
        private readonly Transform transform;

        private Vector3 groundOldPosition;
        
        public TweenableModule(PhysicsModule physics, Transform transform)
        {
            this.physics = physics;
            this.transform = transform;
            physics.onGrounded += HandleOnGroundedEvent;
        }

        public void Update(float deltaTime)
        {
            if (!physics.IsGrounded) return;
            transform.position += physics.Ground.position - groundOldPosition;
            groundOldPosition = physics.Ground.position;
        }

        private void HandleOnGroundedEvent()
        {            
            groundOldPosition = physics.Ground.position;
        }
        
        public void Dispose()
        {
            if (physics != null) physics.onGrounded -= HandleOnGroundedEvent;
        }
    }
}

