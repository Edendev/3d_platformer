using Game.Transformables;
using UnityEngine;

namespace Game.Interaction
{
    public class DisableEntityInteractionAction : IInteractionAction
    {
        private bool hasBeenDisabled = false;
        public void Initialize() { }

        public void Perform(GameObject interactable, GameObject interactor) {
            if (!interactable.activeSelf) return;
            interactable.SetActive(false);
            hasBeenDisabled = true;
        }

        public void Reset(GameObject interactable) {
            if (!hasBeenDisabled) return;
            interactable.SetActive(true);
            hasBeenDisabled = false;
        }
    }
}
