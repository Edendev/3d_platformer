using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class PlayerIdlState : PlayerState
    {
        public PlayerIdlState(uint id, string name, StateMachine stateMachine, Animator animator) : base(id, name, stateMachine, animator) { }
        public override System.Type GetType() => typeof(PlayerIdlState);

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (horizontal != 0f || vertical != 0f)
            {
                StateMachine.ChangeState(StateDefinitions.Player.Walking);
            }
        }
    }
}
