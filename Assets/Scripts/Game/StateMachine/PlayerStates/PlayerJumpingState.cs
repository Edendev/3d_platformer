using Game.Settings;
using Game.PhysicsSystem;
using Game.Utiles;
using Game.Player;
using Game.Interaction;
using UnityEngine;
using Game.Systems;

namespace Game.States
{
    public class PlayerJumpingState : PlayerState
    {
        private readonly float maxJumpHeight;
        private readonly float maxJumpSpeed;
        private readonly AnimationCurve jumpSpeedCurve;
        private readonly float jumpMoveSpeed;
        private readonly float rotationSpeed;
        private readonly float minJumpTime;
        private readonly float decelerationTime;
        private readonly Transform cameraTransform;
        
        private Modifier<float> gravityModifier;
        private float startHeight;
        private float currentJumpSpeed;
        private float jumpTimer;
        private float decelerationTimer;
        private float finalJumpSpeed;
        private bool isStopping = false;
        private KeyCode jumpKey;

        public PlayerJumpingState(uint id, string name, StateMachine stateMachine, Animator animator, InteractionModule interaction, PhysicsModule physics, 
            PlayerSO playerSO, Transform cameraTransform, SettingsSystem settings) 
            : base(id, name, stateMachine, animator, interaction, physics, settings) {
            this.maxJumpHeight = playerSO.MaxJumpHeight;
            this.jumpMoveSpeed = playerSO.WalkingSpeed * playerSO.JumpMoveSpeedModifier;
            this.maxJumpSpeed = playerSO.MaxJumpSpeed;
            this.jumpSpeedCurve = playerSO.JumpSpeedCurve;
            this.rotationSpeed = playerSO.RotationSpeed;
            this.minJumpTime = playerSO.MinJumpTime;
            this.decelerationTime = playerSO.DecelerationTime;
            this.gravityModifier = new Modifier<float>(0.5f, new MultiplyFloatModification(), this);
            this.cameraTransform = cameraTransform;
            settings.TryGetActionKey(EPlayerAction.Jump, out jumpKey);
        }

        public override void Enter() {
            base.Enter();
            physics.AddGravityModifier(gravityModifier);
            isStopping = false;
            jumpTimer = Time.time;
            startHeight = animator.transform.position.y;
            currentJumpSpeed = jumpSpeedCurve.Evaluate(0f) * maxJumpSpeed;
            animator?.SetBool("isJumping", true);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
        
            float deltaHeight = animator.transform.position.y - startHeight;
            bool hasReachedMinJumpTime = Time.time - jumpTimer >= minJumpTime; // make sure it performs a minimum jump when pressing jump for a very short time

            if (!isStopping && (deltaHeight >= maxJumpHeight || (hasReachedMinJumpTime && !Input.GetKey(jumpKey)))) {
                isStopping = true;
                decelerationTimer = Time.time;
                finalJumpSpeed = currentJumpSpeed;
            }

            if (Mathf.Approximately(currentJumpSpeed, 0f)) {
                StateMachine.ChangeState(StateDefinitions.Player.Idl);
                return;
            }

            if (isStopping) {
                // smoothly decelerate
                float blend = (Time.time - decelerationTimer) / decelerationTime;
                currentJumpSpeed = Mathf.Lerp(finalJumpSpeed, 0f, blend);
            } else {
                float heightFraction = deltaHeight / maxJumpHeight;
                currentJumpSpeed = jumpSpeedCurve.Evaluate(heightFraction) * maxJumpSpeed;
            }

            MoveTowards(Vector3.up, currentJumpSpeed, deltaTime);

            // allow horizontal movement while jumping
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (!Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f)) {
                Vector3 move = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
                move.y = 0f;
                move = move != Vector3.zero ? move.normalized : move;
                MoveTowards(move, jumpMoveSpeed, deltaTime);
                FaceTowards(move, rotationSpeed, deltaTime);
            }

            interaction.TouchInteract(); // only allow touch interact while jumping
        }

        public override void Exit() {
            physics.RemoveGravityModifier(this);
            animator?.SetBool("isJumping", false);
            base.Exit();
        }
    }
}
