using Game.PhysicsSystem;
using Game.Player;
using Game.Settings;
using Game.Interaction;
using UnityEngine;
using Game.Systems;

namespace Game.States
{
    public class PlayerWalkingState : PlayerState
    {
        private readonly float walkingSpeed = 1f;
        private readonly float rotationSpeed = 1f;
        private readonly Transform cameraTransform;

        private KeyCode jumpKey;

        public PlayerWalkingState(uint id, string name, StateMachine stateMachine, Animator animator, InteractionModule interaction, PhysicsModule physics, 
            PlayerSO playerSO, Transform cameraTransform, SettingsSystem settings) 
            : base(id, name, stateMachine, animator, interaction, physics, settings) {
            this.walkingSpeed = playerSO.WalkingSpeed;
            this.rotationSpeed = playerSO.RotationSpeed;
            this.cameraTransform = cameraTransform;
            settings.TryGetActionKey(EPlayerAction.Jump, out jumpKey);
        }

        public override void Enter() {
            base.Enter();
            animator?.SetBool("isWalking", true);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            if (physics.IsGrounded && Input.GetKey(jumpKey)) {
                StateMachine.ChangeState(StateDefinitions.Player.Jumping);
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f)) {
                StateMachine.ChangeState(StateDefinitions.Player.Idl);
                return;
            }

            Vector3 move = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
            move.y = 0f;
            move = move != Vector3.zero ? move.normalized : move;
            MoveTowards(move, walkingSpeed, deltaTime);
            FaceTowards(move, rotationSpeed, deltaTime);

            interaction.TouchInteract();
            interaction.InputInteract();
        }

        public override void Exit() {
            animator?.SetBool("isWalking", false);
            base.Exit();
        }
    }
}
