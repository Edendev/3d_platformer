using Game.Transformables;
using Game.Utils;
using UnityEngine;

namespace Game.Interaction
{
    public class InteractableBehaviour : MonoBehaviour
    {
        [SerializeReference, SubclassSelectorProperty] private IInteractionAction[] actions;

        public void Interact(GameObject interactor) {
            if (actions == null) return;
            foreach(IInteractionAction action in actions) {
                action.Perform(gameObject, interactor);
            }
        }
    }
}