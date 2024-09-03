using Game.Transformables;
using UnityEngine;

namespace Game.Interaction
{
    public class PlayTransformableInteractionAction : IInteractionAction
    {
        [SerializeField] private TransformableBehaviour transformable;
        [SerializeField] private bool playSwapped = false;

        private bool currentReversedState = false;

        public void Initialize() { }
        public void Perform(GameObject interactable, GameObject interactor) {
            if (transformable == null) return;
            if (playSwapped) {                
                transformable.TryPlay(currentReversedState);
                currentReversedState = !currentReversedState;
            }
            else transformable.TryPlay();
        }
        public void Reset(GameObject interactable) {
            currentReversedState = false;
            transformable?.TryStop();
        }
    }
}
