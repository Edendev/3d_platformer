using System;
using UnityEngine;

namespace Game.Transformables
{
    [Serializable]
    public class TranslateTransformableAction : ITransformableAction
    {
        public event System.Action onStarted;
        public event System.Action onStopped;
        public event System.Action onFinished;

        [SerializeField, Tooltip("Destination in local coordinates")] private Vector3 destination;
        [SerializeField] private float speed;
        public Vector3 EndPosition => destination;
        public Quaternion EndRotation { get; } = Quaternion.identity;
        public Vector3 EndScale { get; } = new Vector3(1f, 1f, 1f);

        private TransformableBehaviour transformable;
        private Vector3 worldDestination;

        public void Begin(TransformableBehaviour transformable, bool reversed = false) {
            this.transformable = transformable;
            worldDestination = reversed ? transformable.GetStartPointForCurrentAction() : transformable.GetStartPointForCurrentAction() + destination;
            onStarted?.Invoke();
        }
        public void Update(float deltaTime) {
            Vector3 direction = (worldDestination - transformable.transform.position);
            float currentTranslationStep = speed * deltaTime;
            if (direction.magnitude <= currentTranslationStep)
            {
                transformable.transform.position = worldDestination;
                onFinished?.Invoke();
                return;
            }
            transformable.transform.position = transformable.transform.position + direction.normalized * currentTranslationStep;
        }
        public void Stop() { }
    }
}
