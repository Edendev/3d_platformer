using Game.Settings;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class PlayerWalkingState : PlayerState
    {
        private readonly float walkingSpeed = 1f;
        private readonly float rotationSpeed = 1f;
        public PlayerWalkingState(uint id, string name, StateMachine stateMachine, Animator animator) : base(id, name, stateMachine, animator) {
            GameSettingsSO gameSettingsSO = GameManager.Instance.GameSOContainer.GameSettingsSO;
            this.walkingSpeed = gameSettingsSO.PlayerSO.WalkingSpeed;
            this.rotationSpeed = gameSettingsSO.PlayerSO.RotationSpeed;
        }
        public override System.Type GetType() => typeof(PlayerWalkingState);

        public override void Enter()
        {
            base.Enter();
            animator?.SetBool("isWalking", true);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (horizontal == 0f && vertical == 0f)
            {
                StateMachine.ChangeState(StateDefinitions.Player.Idl);
                return;
            }
            Vector3 move = new Vector3(horizontal, 0f, vertical);
            animator.transform.Translate(move * walkingSpeed * deltaTime);
            Quaternion faceRotation = Quaternion.FromToRotation(animator.transform.forward, move);
            //animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, faceRotation, rotationSpeed * deltaTime);
        }

        public override void Exit()
        {
            animator?.SetBool("isWalking", false);
            base.Exit();
        }
    }
}
