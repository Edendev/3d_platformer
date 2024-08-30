using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.PhysicsSystem;

namespace Game.States
{
    public class PlayerIdlState : PlayerState
    {
        public PlayerIdlState(uint id, string name, StateMachine stateMachine, Animator animator, PhysicsModule physics) : base(id, name, stateMachine, animator, physics) { }
        public override System.Type GetType() => typeof(PlayerIdlState);

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (!Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f)) {
                StateMachine.ChangeState(StateDefinitions.Player.Walking);
            }

            if (physics.IsGrounded && Input.GetKey(KeyCode.Space)) {
                StateMachine.ChangeState(StateDefinitions.Player.Jumping);
            }
        }
    }
}
