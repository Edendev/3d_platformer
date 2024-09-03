using Game.Systems;
using UnityEngine;

namespace Game.Interaction
{
    /// <summary>
    ///  Handles the player interaction with the scene interactables
    /// </summary>
    public class InteractionModule
    {
        private readonly GameObject interactor;
        private readonly SettingsSystem settings;
        private readonly float range;

        private int interactableLayerMask;
        private float interactionDelay = 0.2f;
        private float interactionDelayTimer;
        private KeyCode interactKey;

        public InteractionModule(GameObject interactor, float range, SettingsSystem settings)
        {
            this.interactor = interactor;
            this.settings = settings;
            this.range = range;
            this.interactableLayerMask = LayerMask.GetMask("Interactable");
            settings.TryGetActionKey(Player.EPlayerAction.Interact, out interactKey);
            interactionDelayTimer = Time.time;
        }

        public void TouchInteract() {
            if (Time.time - interactionDelayTimer < interactionDelay) return;
            interactionDelayTimer = Time.time;
            TryInteract(EInteractionType.Touch);
        }

        public void InputInteract() {
            if (Input.GetKeyDown(interactKey)) {
                TryInteract(EInteractionType.Input);
            }
        }

        private void TryInteract(EInteractionType interactionType) {
            Collider[] interactables = Physics.OverlapSphere(interactor.transform.position, range, interactableLayerMask);
            if (interactables.Length == 0) return;
            foreach (Collider collider in interactables) {
                if (!collider.gameObject.activeSelf) continue;
                InteractableBehaviour interactable = collider.GetComponent<InteractableBehaviour>();
                if (interactable == null) continue;
                if (interactable.InteractionType != interactionType) continue;
                interactable.Interact(interactor);
            }
        }
    }
}

