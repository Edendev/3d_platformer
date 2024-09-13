using UnityEngine;
using Game.Utils;
using System;

namespace Game.Transformables
{
    /// <summary>
    /// Provides functionality to perform transformation actions in a specific order.
    /// </summary>
    public class TransformableBehaviour : MonoBehaviour
    {
        public event Action<TransformableBehaviour> onStarted;
        public event Action<int> onStopped;        

        [SerializeReference, SubclassSelectorProperty] public ITransformableAction[] transformableActions;
        [SerializeField] private ETransformableStartTrigger startTrigger;
        [SerializeField] private bool playInLoop;

        public ETransformableStartTrigger StartTrigger => startTrigger;
        public int ActiveContainerIndex { get; set; } = -1; // index set by the TransformablesSystem to keep track of the position in the container (array)

        private int currentActionIndex = 0;
        private bool playReversed = false;
        private Vector3 initialWorldPosition;
        private Quaternion initialRotation;

        private void Start() {
            // cache initial state
            initialWorldPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void FrameUpdate(float deltaTime) {
            transformableActions[currentActionIndex].Update(Time.deltaTime);
        }

        public void Initialize() {
            foreach(ITransformableAction action in transformableActions) {
                action.Initialize();
            }
        }

        public bool TryPlay(bool reversed = false) {
            // If the transformable was active, check if we are trying to play in a different reversed state
            if (ActiveContainerIndex != -1) { 
                if (reversed != playReversed) {
                    TryStopTransformableActions();
                } else {
                    return true;
                }
            }
            playReversed = reversed;
            return TryStartTransformableActions();

        }

        public bool TryStop() {
            return TryStopTransformableActions();
        }

        private bool TryStartTransformableActions() {
            if (transformableActions.Length == 0) return false;
            PerformCurrentAction();
            onStarted?.Invoke(this);
            return true;
        }

        private bool TryStopTransformableActions() {
            if (transformableActions.Length == 0) return false;
            transformableActions[currentActionIndex].onStopped -= HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            transformableActions[currentActionIndex].Stop();
            onStopped?.Invoke(ActiveContainerIndex);
            return true;
        }

        private void PerformCurrentAction() {
            if (currentActionIndex < 0 || currentActionIndex >= transformableActions.Length) return; // safety check
            transformableActions[currentActionIndex].onStopped -= HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            transformableActions[currentActionIndex].onStopped += HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished += HandleOnActionFinished;
            transformableActions[currentActionIndex].Begin(this, playReversed);
        }

        private void PerformNextAction() {
            bool isAtLastAction = currentActionIndex == 0 || currentActionIndex == transformableActions.Length - 1;
            if (!playInLoop && isAtLastAction)
            {
                onStopped?.Invoke(ActiveContainerIndex);
                return;
            }
            currentActionIndex = playReversed ? currentActionIndex - 1 : currentActionIndex + 1;
            if (currentActionIndex < 0) currentActionIndex = transformableActions.Length - 1;
            else if (currentActionIndex > transformableActions.Length - 1) currentActionIndex = 0;
            PerformCurrentAction();
        }

        private void HandleOnActionStopped() {
            transformableActions[currentActionIndex].onStopped -= HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            PerformNextAction();
        }

        private void HandleOnActionFinished() {
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            PerformNextAction();
        }

        public void DoReset() {
            currentActionIndex = 0;
            transform.position = initialWorldPosition;
            transform.rotation = initialRotation;
            foreach (ITransformableAction action in transformableActions)
            {
                action.onStopped -= HandleOnActionStopped;
                action.onFinished -= HandleOnActionFinished;
            }
            // start immediately if the trigger requires it
            if (startTrigger == ETransformableStartTrigger.OnEnable)
            {
                PerformCurrentAction();
            }
        }

        /// <summary>
        /// Action by action reconstruct the path to know the exact start position of a specific action
        /// </summary>
        public Vector3 GetStartPointForCurrentAction() {
            Vector3 startPoint = initialWorldPosition;
            for (int i = 0; i < currentActionIndex; i++) {
                startPoint += transformableActions[i].EndPosition;
            }
            return startPoint;
        }

        /// <summary>
        /// Action by action reconstruct the path to know the exact start rotation of a specific action
        /// </summary>
        public Quaternion GetStartRotationForCurrentAction() {
            Quaternion startRotation = initialRotation;
            for (int i = 0; i < currentActionIndex; i++) {
                startRotation *= transformableActions[i].EndRotation; 
            }
            return startRotation;
        }
    }
}
