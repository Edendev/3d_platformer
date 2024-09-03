using Game.Transformables;
using UnityEngine;

namespace Game.Interaction
{
    public class PlayTransformableInteractionAction : IInteractionAction
    {
        [SerializeField] private TransformableBehaviour transformable;
        [SerializeField] private bool playSwapped = false;
        public void Initialize() { }
        public void Perform(GameObject interactable, GameObject interactor) {
            if (playSwapped) transformable?.TryPlaySwapped();
            else transformable?.TryPlay();
        }
        public void Reset(GameObject interactable) {
            transformable?.TryStop();
        }
    }
}
