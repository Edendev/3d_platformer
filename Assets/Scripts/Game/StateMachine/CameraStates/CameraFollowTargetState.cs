using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.PhysicsSystem;
using Game.Interfaces;
using Game.Settings;
using UnityEngine.Jobs;
using Game.CameraControl;
using Game.Utiles;

namespace Game.States
{
    public class CameraFollowTargetState : CameraState
    {
        private const float FOLLOW_DISTANCE_THRESHOLD = 0.2f;

        private readonly Vector3 targetOffset;
        private readonly Quaternion orientation;
        private readonly float movementSpeed;
        private readonly float rotationSpeed;

        private IPosition target;
        private ObstacleAvoidanceModule obstacleAvoidanceModule;

        public CameraFollowTargetState(uint id, string name, StateMachine stateMachine, Transform transform, ObstacleAvoidanceModule obstacleAvoidanceModule, CameraSO cameraSO) : base(id, name, stateMachine, transform) { 
            this.obstacleAvoidanceModule = obstacleAvoidanceModule;
            this.targetOffset = cameraSO.TargetOffset;
            this.movementSpeed = cameraSO.MovementSpeed;
            this.rotationSpeed = cameraSO.RotationSpeed;
            this.orientation = Quaternion.Euler(cameraSO.Orientation);
        }

        public override System.Type GetType() => typeof(CameraFollowTargetState);

        public void SetTarget(IPosition target) {
            this.target = target;
        }

        public override void Enter()
        {
            base.Enter();
            if (target == null) {
                StateMachine.ChangeState(StateDefinitions.Camera.UI);
                return;
            }
        }

        public override void Update(float deltaTime)
        {
            Vector3 move = (target.Position + targetOffset + obstacleAvoidanceModule.CurrentAvoidanceOffset) - transform.position;
            if (move.magnitude > FOLLOW_DISTANCE_THRESHOLD) {
                move = move != Vector3.zero ? move.normalized : move;
                transform.position = transform.position + move * movementSpeed * deltaTime;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientation, rotationSpeed * deltaTime);
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            obstacleAvoidanceModule.Update(deltaTime);
        }
    }
}
