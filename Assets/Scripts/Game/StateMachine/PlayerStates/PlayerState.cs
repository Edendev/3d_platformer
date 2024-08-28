using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public abstract class PlayerState : State
    {
        protected readonly Animator animator;

        public PlayerState(uint id, string name, StateMachine stateMachine, Animator animator) : base(id, name, stateMachine) { 
            this.animator = animator;
        }
        public override System.Type GetType() => typeof(PlayerIdlState);
    }
}
