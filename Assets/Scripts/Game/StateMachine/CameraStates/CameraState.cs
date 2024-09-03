using UnityEngine;

namespace Game.States
{
    public abstract class CameraState : State
    {
        protected readonly Transform transform;

        public CameraState(uint id, string name, StateMachine stateMachine, Transform transform) : base(id, name, stateMachine)
        {
            this.transform = transform;
        }
    }
}
