using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.PhysicsSystem;
using Game.Interfaces;
using Game.Settings;
using UnityEngine.Jobs;
using Game.CameraControl;

namespace Game.States
{
    public class CameraUIState : CameraState
    {
        private readonly Vector3 targetPosition;
        private readonly Vector3 targetRotation;
        private readonly float movementSpeedMultiplier;
        private readonly float rotationSpeed;

        private Quaternion targetOrientation;
        public CameraUIState(uint id, string name, StateMachine stateMachine, Transform transform, GameSettingsSO gameSettingsSO) : base(id, name, stateMachine, transform)
        {
            this.targetPosition = gameSettingsSO.CameraUIPosition;
            this.targetRotation = gameSettingsSO.CameraUIRotation;
            this.movementSpeedMultiplier = gameSettingsSO.CameraSO.MovementSpeedMultiplier;
            this.rotationSpeed = gameSettingsSO.CameraSO.RotationSpeed;
        }

        public override System.Type GetType() => typeof(CameraFollowTargetState);

        public override void Enter()
        {
            base.Enter();

            targetOrientation = Quaternion.Euler(targetRotation);
        }
        public override void Update(float deltaTime)
        {
            Vector3 move = targetPosition - transform.position;
            transform.position = transform.position + move * movementSpeedMultiplier * deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, rotationSpeed * deltaTime);
        }
    }
}
