using UnityEngine;

namespace Game.PhysicsSystem
{
    public class TriggerEventsAnnouncer : MonoBehaviour
    {
        public event System.Action<Collider> onTriggerEnter;
        public event System.Action<Collider> onTriggerExit;
        
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other);
        }
    }
}
