using System;
using Game.PhysicsSystem;
using UnityEngine;

namespace Game.Player
{
    /// <summary>
    /// Handles any trigger that causes the death of the player.
    /// </summary>
    public class PlayerDeathModule : IDisposable
    {
        public event Action onDeath;

        private readonly TriggerEventsAnnouncer triggerEventsAnnouncer;
        private readonly CollisionEventsAnnouncer collisionEventsAnnouncer;
        private readonly int obstacleLayer;

        public PlayerDeathModule(TriggerEventsAnnouncer triggerEventsAnnouncer, CollisionEventsAnnouncer collisionEventsAnnouncer)
        {
            this.triggerEventsAnnouncer = triggerEventsAnnouncer;
            this.collisionEventsAnnouncer = collisionEventsAnnouncer;
            this.obstacleLayer = LayerMask.NameToLayer("Obstacle");
            triggerEventsAnnouncer.onTriggerEnter += HandleOnTriggerEnterEvent;
            collisionEventsAnnouncer.onCollisionEnter += HandleOnCollisionEnterEvent;
        }

        private void HandleOnTriggerEnterEvent(Collider other) {
            if (other.gameObject.layer != obstacleLayer) return;
            onDeath?.Invoke();
        }

        private void HandleOnCollisionEnterEvent(Collider other) {
            if (other.gameObject.layer != obstacleLayer) return;
            onDeath?.Invoke();
        }
        public void Dispose() {
            triggerEventsAnnouncer.onTriggerEnter -= HandleOnTriggerEnterEvent;
            collisionEventsAnnouncer.onCollisionEnter -= HandleOnCollisionEnterEvent;
        }
    }
}

