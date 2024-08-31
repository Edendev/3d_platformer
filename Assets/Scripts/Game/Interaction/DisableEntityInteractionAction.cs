
using Game.Transformables;
using UnityEngine;

namespace Game.Interaction
{
    public class DisableEntityInteractionAction : IInteractionAction
    {
        public void Initialize() { }
        public void Perform(GameObject interactable, GameObject interactor)
        {
            interactable.SetActive(false);
        }

        public void Reset(GameObject interactable) {
            interactable.SetActive(true);
        }
    }
}
