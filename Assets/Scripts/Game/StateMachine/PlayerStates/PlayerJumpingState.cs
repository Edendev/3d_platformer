using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class PlayerJumpingState : PlayerState
    {
        public PlayerJumpingState(uint id, string name, StateMachine stateMachine, Animator animator) : base(id, name, stateMachine, animator) { }
        public override System.Type GetType() => typeof(PlayerJumpingState);
    }
}
