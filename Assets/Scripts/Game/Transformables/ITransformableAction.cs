using UnityEngine;

namespace Game.Transformables
{
    public interface ITransformableAction
    {
        event System.Action onStarted;
        event System.Action onStopped;
        event System.Action onFinished;

        void Initialize();
        void Begin(TransformableBehaviour transformable, bool reversed = false);
        void Update(float deltaTime);
        void Stop();
        Vector3 EndPosition { get; }
        Quaternion EndRotation { get; }
    }
}
