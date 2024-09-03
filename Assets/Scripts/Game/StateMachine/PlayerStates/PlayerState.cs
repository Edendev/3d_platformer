using Game.PhysicsSystem;
using Game.Interaction;
using UnityEngine;
using Game.Systems;

namespace Game.States
{
    public abstract class PlayerState : State
    {
        protected readonly Animator animator;
        protected readonly InteractionModule interaction;
        protected readonly SettingsSystem settings;
        protected readonly PhysicsModule physics;

        public PlayerState(uint id, string name, StateMachine stateMachine, Animator animator, InteractionModule interaction, PhysicsModule physics, SettingsSystem settings) 
            : base(id, name, stateMachine) {
            this.animator = animator;
            this.interaction = interaction;
            this.physics = physics; 
            this.settings = settings;
        }

        protected void MoveTowards(Vector3 direction, float speed, float deltaTime) {
            animator.transform.position = animator.transform.position + direction * speed * deltaTime;
        }

        protected void FaceTowards(Vector3 direction, float speed, float deltaTime) {
            Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
            animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, lookAtRotation, speed * deltaTime);
        }
    }
}
