using System;
using UnityEngine;

namespace Game.Transformables
{
    [Serializable]
    public class RotateTransformableAction : ITransformableAction
    {
        public event System.Action onStarted;
        public event System.Action onStopped;
        public event System.Action onFinished;

        [SerializeField] private float speed;
        [SerializeField, Tooltip("Rotation axis")] private Vector3 axis;
        [SerializeField] private bool rotateConstantly;
        [SerializeField, Range(0f, 360), Tooltip("Rotation degrees if not rotate constantly")] private float degrees;

        public Vector3 EndPosition => Vector3.zero;
        public Quaternion EndRotation => endRotation;
        private Quaternion endRotation;

        private TransformableBehaviour transformable;
        private float currentRotationAngle = 0;
        private float rotationDirection;
        private Quaternion targetRotation;
        private float targetRotationAngle;

        public void Initialize() {
            this.endRotation = Quaternion.AngleAxis(degrees, axis);
        }

        public void Begin(TransformableBehaviour transformable, bool reversed = false) {
            this.transformable = transformable;
            currentRotationAngle = 0f;
            rotationDirection = reversed ? -1f : 1f;
            targetRotation = transformable.GetStartRotationForCurrentAction();
            if (!reversed) targetRotation *= EndRotation;
            targetRotationAngle = Quaternion.Angle(transformable.transform.rotation, targetRotation);
            onStarted?.Invoke();
        }

        public void Update(float deltaTime) {
            
            if (!rotateConstantly && currentRotationAngle >= targetRotationAngle) {
                onFinished?.Invoke();
                return;
            }

            float deltaRotation = speed * deltaTime;

            if (!rotateConstantly) {
                // make sure we do not rotate more than the target rotation
                currentRotationAngle += deltaRotation;
                if (currentRotationAngle >= targetRotationAngle) {
                    deltaRotation -= targetRotationAngle - currentRotationAngle;
                }
            }
            if (rotateConstantly) transformable.transform.rotation *= Quaternion.AngleAxis(rotationDirection * deltaRotation, axis);
            else transformable.transform.rotation = Quaternion.RotateTowards(transformable.transform.rotation, targetRotation, deltaRotation);
        }

        public void Stop() => onStopped?.Invoke();
    }
}
     