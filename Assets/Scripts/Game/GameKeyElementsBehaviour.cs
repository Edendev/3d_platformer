using Game.Interaction;
using Game.PhysicsSystem;
using Game.Transformables;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Container of all necessary scene references.
    /// </summary>
    public class GameKeyElementsBehaviour : MonoBehaviour
    {
        public TriggerEventsAnnouncer LevelCompletedTrigger => levelCompletedTrigger;
        [SerializeField] private TriggerEventsAnnouncer levelCompletedTrigger;

        public TransformableBehaviour[] Transformables => transformables;
        public InteractableBehaviour[] Interactables => interactables;

        // Cached scene references 
        [SerializeField] private TransformableBehaviour[] transformables;
        [SerializeField] private InteractableBehaviour[] interactables;

        private void Awake()
        {
            // collect all scene references
            interactables = GameObject.FindObjectsOfType<InteractableBehaviour>();
            transformables = GameObject.FindObjectsOfType<TransformableBehaviour>();
        }
    }
}
