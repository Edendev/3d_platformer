using Game.Interaction;

namespace Game.Systems
{
    /// <summary>
    /// Provides some functionality to communicate with all interactables in the scene
    /// </summary>
    public class InteractablesSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private readonly InteractableBehaviour[] interactables;
        public InteractablesSystem(InteractableBehaviour[] interactables)
        {
            this.interactables = interactables;
            foreach (InteractableBehaviour interactable in interactables) {
                interactable.Initialize();
            }
        }

        public void Destroy() { }

        public void ResetAllInteractables()
        {
            foreach (InteractableBehaviour interactable in interactables) {
                interactable.DoReset();
            }
        }
    }
}
