
using Game.Transformables;
using UnityEngine;

namespace Game.Interaction
{
    public class DisableEntityInteractionAction : IInteractionAction
    {
        public void Perform(GameObject interactable, GameObject interactor)
        {
            interactable.SetActive(false);
        }
    }
}
