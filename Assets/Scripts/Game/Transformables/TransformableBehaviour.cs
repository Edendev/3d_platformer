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

        public int ActiveContainerIndex { get; set; }

        private int currentActionIndex = 0;
        private bool playReversed;
        private Vector3 initialWorldPosition;

        private void Start()
        {
            initialWorldPosition = transform.position;
        }

        public bool TryPlay(bool reversed = false)
        {
            playReversed = reversed;
            return TryStartTransformableActions();

        }
        public bool TryPlaySwapped()
        {
            playReversed = !playReversed; 
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
            onStarted?.Invoke(this);
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
            currentActionIndex = playReversed ? currentActionIndex - 1 : currentActionIndex + 1;
            bool isAtLastAction = currentActionIndex < 0 || currentActionIndex > transformableActions.Length - 1;
            if (!playInLoop && isAtLastAction) {
                onStopped?.Invoke(ActiveContainerIndex);
                return;
            }
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
                startPoint += transformableActions[i].EndPosition; // add all endPoints action by action
            }
            return startPoint;
        }
    }
}
