using UnityEngine;

namespace Game.PhysicsSystem
{
    public class CollisionEventsAnnouncer : MonoBehaviour
    {
        public event System.Action<Collider> onCollisionEnter;
        public event System.Action<Collider> onCollisionExit;

        private void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter?.Invoke(collision.collider);
        }

        private void OnCollisionExit(Collision collision)
        {
            onCollisionExit?.Invoke(collision.collider);
        }
    }
}
