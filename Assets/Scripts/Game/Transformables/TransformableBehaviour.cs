using UnityEngine;
using Game.Utils;
using System;

namespace Game.Transformables
{
    public class TransformableBehaviour : MonoBehaviour
    {
        public event Action<TransformableBehaviour> onStarted;
        public event Action<int> onStopped;

        public enum ETransformableStartTrigger
        {
            OnEnable,
            OnRequest
        }

        [SerializeReference, SubclassSelectorProperty] public ITransformableAction[] transformableActions;
        [SerializeField] private ETransformableStartTrigger startTrigger;
        [SerializeField] private bool playInLoop;

        public ETransformableStartTrigger StartTrigger => startTrigger;

        public int ActiveContainerIndex { get; set; } = -1;

        private int currentActionIndex = 0;
        private bool playReversed = false;
        private Vector3 initialWorldPosition;
        private Quaternion initialRotation;

        private void Start()
        {
            initialWorldPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Initialize()
        {
            foreach(ITransformableAction action in transformableActions) {
                action.Initialize();
            }
        }

        public bool TryPlay(bool reversed = false)
        {
            if (ActiveContainerIndex != -1)
            {
                if (reversed != playReversed)
                {
                    TryStopTransformableActions();
                }
                else
                {
                    return true;
                }
            }
            playReversed = reversed;
            return TryStartTransformableActions();

        }
        public bool TryPlaySwapped()
        {
            playReversed = !playReversed;
            if (ActiveContainerIndex != -1) {
                TryStopTransformableActions();
            }
            return TryStartTransformableActions();
        }

        public bool TryStop()
        {
            return TryStopTransformableActions();
        }

        public void DoReset()
        {
            currentActionIndex = 0;
            transform.position = initialWorldPosition;
            transform.rotation = initialRotation;
            foreach(ITransformableAction action in transformableActions) {
                action.onStopped -= HandleOnActionStopped;
                action.onFinished -= HandleOnActionFinished;
            }
            if (startTrigger == ETransformableStartTrigger.OnEnable) {
                PerformCurrentAction();
            }
        }

        public void FrameUpdate(float deltaTime)
        {
            transformableActions[currentActionIndex].Update(Time.deltaTime);
        }

        private bool TryStartTransformableActions()
        {
            if (transformableActions.Length == 0) return false;
            PerformCurrentAction();
            onStarted?.Invoke(this);
            return true;

        }
        private bool TryStopTransformableActions()
        {
            if (transformableActions.Length == 0) return false;
            transformableActions[currentActionIndex].onStopped -= HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            transformableActions[currentActionIndex].Stop();
            onStopped?.Invoke(ActiveContainerIndex);
            return true;
        }

        private void PerformCurrentAction()
        {
            if (currentActionIndex < 0 || currentActionIndex >= transformableActions.Length) return; // safety check
            transformableActions[currentActionIndex].onStopped -= HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            transformableActions[currentActionIndex].onStopped += HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished += HandleOnActionFinished;
            transformableActions[currentActionIndex].Begin(this, playReversed);
        }
        private void HandleOnActionStopped()
        {
            transformableActions[currentActionIndex].onStopped -= HandleOnActionStopped;
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            PerformNextAction();
        }

        private void HandleOnActionFinished()
        {
            transformableActions[currentActionIndex].onFinished -= HandleOnActionFinished;
            PerformNextAction();
        }

        private void PerformNextAction()
        {
            bool isAtLastAction = currentActionIndex == 0 || currentActionIndex == transformableActions.Length - 1;
            if (!playInLoop && isAtLastAction) {
                onStopped?.Invoke(ActiveContainerIndex);
                return;
            }
            currentActionIndex = playReversed ? currentActionIndex - 1 : currentActionIndex + 1;
            if (currentActionIndex < 0) currentActionIndex = transformableActions.Length - 1;
            else if (currentActionIndex > transformableActions.Length - 1) currentActionIndex = 0;
            PerformCurrentAction();
        }

        /// <summary>
        /// Action by action reconstruct the path to know the exact start position of a specific action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Vector3 GetStartPointForCurrentAction()
        {
            Vector3 startPoint = initialWorldPosition;
            for (int i = 0; i < currentActionIndex; i++)
            {
                startPoint += transformableActions[i].EndPosition;
            }
            return startPoint;
        }

        /// <summary>
        /// Action by action reconstruct the path to know the exact start rotation of a specific action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Quaternion GetStartRotationForCurrentAction()
        {
            Quaternion startRotation = initialRotation;
            for (int i = 0; i < currentActionIndex; i++)
            {
                startRotation *= transformableActions[i].EndRotation; 
            }
            return startRotation;
        }
    }
}
