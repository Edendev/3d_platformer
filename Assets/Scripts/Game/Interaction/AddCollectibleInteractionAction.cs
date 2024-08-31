
using Game.Systems;
using Game.Transformables;
using UnityEngine;

namespace Game.Interaction
{
    public class AddCollectibleInteractionAction : IInteractionAction
    {
        private CollectiblesSystem collectiblesSystem;

        private int collectiblesAddedCount = 0;

        public void Initialize()
        {
            GameManager.Instance.TryGetSystem<CollectiblesSystem>(out collectiblesSystem);
        }
        public void Perform(GameObject interactable, GameObject interactor)
        {
            collectiblesSystem?.AddCollectible();
            collectiblesAddedCount++;
        }
        public void Reset(GameObject interactable)
        {
            collectiblesSystem?.RemoveCollectibles(collectiblesAddedCount);
            collectiblesAddedCount = 0;
        }
    }
}
