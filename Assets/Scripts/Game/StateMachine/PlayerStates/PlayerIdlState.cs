using Game.Interaction;
using Game.Systems;
using Game.PhysicsSystem;
using Game.Player;
using UnityEngine;

namespace Game.States
{
    public class PlayerIdlState : PlayerState
    {
        private KeyCode jumpKey;
        public PlayerIdlState(uint id, string name, StateMachine stateMachine, Animator animator, InteractionModule interaction, PhysicsModule physics, SettingsSystem settings) 
            : base(id, name, stateMachine, animator, interaction, physics, settings) {
            settings.TryGetActionKey(EPlayerAction.Jump, out jumpKey);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (!Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f)) {
                StateMachine.ChangeState(StateDefinitions.Player.Walking);
            }

            if (physics.IsGrounded && Input.GetKey(jumpKey)) {
                StateMachine.ChangeState(StateDefinitions.Player.Jumping);
            }

            interaction.TouchInteract();
            interaction.InputInteract();
        }
    }
}
