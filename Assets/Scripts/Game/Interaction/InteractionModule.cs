using Game.PhysicsSystem;
using UnityEngine;

namespace Game.Interaction
{
    public class InteractionModule
    {
        private readonly GameObject interactor;
        private readonly float range;

        private int interactableLayerMask;

        private float interactionDelay = 0.2f;
        private float interactionDelayTimer;
        
        public InteractionModule(GameObject interactor, float range)
        {
            this.interactor = interactor;
            this.range = range;
            this.interactableLayerMask = LayerMask.GetMask("Interactable");
            interactionDelayTimer = Time.time;
        }

        public void Update(float deltaTime)
        {
            if (Time.time - interactionDelayTimer < interactionDelay) return;
            interactionDelayTimer = Time.time;

            Collider[] interactables = Physics.OverlapSphere(interactor.transform.position, range, interactableLayerMask);
            if (interactables.Length == 0) return;
            foreach(Collider collider in interactables) { 
                if (!collider.gameObject.activeSelf) continue;
                InteractableBehaviour interactable = collider.GetComponent<InteractableBehaviour>();
                if (interactable == null) continue;
                interactable.Interact(interactor);
            }
        }
    }
}

