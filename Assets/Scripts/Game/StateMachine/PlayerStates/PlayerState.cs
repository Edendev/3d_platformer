using Game.PhysicsSystem;
using Game.Settings;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public abstract class PlayerState : State
    {
        protected readonly Animator animator;
        protected readonly PhysicsModule physics;

        public PlayerState(uint id, string name, StateMachine stateMachine, Animator animator, PhysicsModule physics) : base(id, name, stateMachine) {
            this.animator = animator;
            this.physics = physics; 
        }
        public override System.Type GetType() => typeof(PlayerIdlState);

        protected void MoveTowards(Vector3 direction, float speed, float deltaTime)
        {
            animator.transform.position = animator.transform.position + direction * speed * deltaTime;
        }

        protected void FaceTowards(Vector3 direction, float speed, float deltaTime)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
            animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, lookAtRotation, speed * deltaTime);
        }
    }
}
