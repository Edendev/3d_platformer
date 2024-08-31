using Game.Transformables;
using Game.Utils;
using UnityEngine;

namespace Game.Interaction
{
    public class InteractableBehaviour : MonoBehaviour
    {
        [SerializeReference, SubclassSelectorProperty] private IInteractionAction[] actions;

        public void Initialize() {
            foreach (IInteractionAction action in actions) {
                action.Initialize();
            }
        }

        public void Interact(GameObject interactor) 
        {
            if (actions == null || actions.Length == 0) return;
            foreach(IInteractionAction action in actions) {
                action.Perform(gameObject, interactor);
            }
        }

        public void DoReset()
        {   
            if (actions == null || actions.Length == 0) return;
            foreach (IInteractionAction action in actions) {
                action.Reset(gameObject);
            }
        }
    }
}