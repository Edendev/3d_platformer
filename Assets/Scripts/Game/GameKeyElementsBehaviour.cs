using Game.PhysicsSystem;
using Game.Transformables;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ExecuteInEditMode]
    public class GameKeyElementsBehaviour : MonoBehaviour
    {
        public TriggerEventsAnnouncer LevelCompletedTrigger => levelCompletedTrigger;
        [SerializeField] private TriggerEventsAnnouncer levelCompletedTrigger;

        public TransformableBehaviour[] Transformables => transformables;

        // Cached scene referenes 
        [SerializeField] private TransformableBehaviour[] transformables;

        private void Awake()
        {
#if UNITY_EDITOR
            EditorSceneManager.sceneSaved += HandleOnSceneSavedEvent;
#endif
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            EditorSceneManager.sceneSaved -= HandleOnSceneSavedEvent;
#endif
        }

#if UNITY_EDITOR
        private void HandleOnSceneSavedEvent(Scene scene)
        {
            transformables = GameObject.FindObjectsOfType<TransformableBehaviour>();
        }
#endif
    }
}
