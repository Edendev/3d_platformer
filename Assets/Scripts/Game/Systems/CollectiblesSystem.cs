using UnityEngine;

namespace Game.Systems
{
    public class CollectiblesSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Public;

        private readonly GameUIBehaviour gameUI;

        private int collectiblesCount = 0;

        public CollectiblesSystem(GameUIBehaviour gameUI)
        {
            this.gameUI = gameUI;
        }

        public void Destroy() { }

        public void AddCollectible()
        {
            collectiblesCount++;
            gameUI.SetCollectibleCount(collectiblesCount);
        }

        public void RemoveCollectibles(int count)
        {
            collectiblesCount -= count;
            gameUI.SetCollectibleCount(collectiblesCount);
        }
    }
}
