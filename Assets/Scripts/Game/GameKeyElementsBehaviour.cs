using Game.PhysicsSystem;
using UnityEngine;

namespace Game
{
    public class GameKeyElementsBehaviour : MonoBehaviour
    {
        public TriggerEventsAnnouncer RestartTrigger => restartTrigger;
        [SerializeField] private TriggerEventsAnnouncer restartTrigger;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (restartTrigger == null)
            {
                Debug.LogError($"{nameof(restartTrigger)} is missing!");
            }
        }
#endif
    }
}
