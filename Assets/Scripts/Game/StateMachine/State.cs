using System;

namespace Game.States
{
    /// <summary>
    /// Base class for any state that can be used in a StateMachine
    /// </summary>
    public abstract class State : IDisposable
    {
        public readonly uint ID;
        public readonly string Name;
        public StateMachine StateMachine => stateMachine;
        private StateMachine stateMachine;
        public State(uint id, string name, StateMachine stateMachine)
        {
            this.ID = id;
            this.Name = name;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float deltaTime) { }
        public virtual void LateUpdate(float deltaTime) { }
        public virtual void Exit() { }
        public virtual void Dispose() {
            stateMachine = null;
        }
    }
}