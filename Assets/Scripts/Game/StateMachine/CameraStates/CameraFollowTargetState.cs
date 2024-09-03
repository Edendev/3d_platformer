using Game.Interfaces;
using Game.CameraControl;
using Game.Utils;
using Game.Settings;
using Game.Player;
using UnityEngine;
using Game.Systems;

namespace Game.States
{
    public class CameraFollowTargetState : CameraState
    {
        private const float FOLLOW_DISTANCE_THRESHOLD = 0.2f;

        private readonly PositionRotation[] targetOffsets;
        private readonly Quaternion[] orientations;
        private readonly float movementSpeedMultiplier;
        private readonly float rotationSpeed;
        private readonly SettingsSystem settings;

        private int currentOffsetIndex = 0;
        private IPosition target;
        private ObstacleAvoidanceModule obstacleAvoidanceModule;
        private KeyCode previousCameraViewKey;
        private KeyCode nextCameraViewKey;

        public CameraFollowTargetState(uint id, string name, StateMachine stateMachine, Transform transform, ObstacleAvoidanceModule obstacleAvoidanceModule, 
            CameraSO cameraSO, SettingsSystem settings) 
            : base(id, name, stateMachine, transform) { 
            this.settings = settings;
            this.obstacleAvoidanceModule = obstacleAvoidanceModule;
            this.targetOffsets = cameraSO.TargetOffsets;
            this.movementSpeedMultiplier = cameraSO.MovementSpeedMultiplier;
            this.rotationSpeed = cameraSO.RotationSpeed;
            this.orientations = new Quaternion[targetOffsets.Length];
            for(int i = 0; i < orientations.Length; i++) {
                orientations[i] = Quaternion.Euler(targetOffsets[i].Rotation);
            }
            settings.TryGetActionKey(EPlayerAction.PreviousCameraView, out previousCameraViewKey);
            settings.TryGetActionKey(EPlayerAction.NextCameraView, out nextCameraViewKey);
        }

        public void SetTarget(IPosition target) {
            this.target = target;
        }

        public override void Enter() {
            base.Enter();
            if (target == null) {
                StateMachine.ChangeState(StateDefinitions.Camera.UI);
                return;
            }
        }

        public override void Update(float deltaTime) {
            if (Input.GetKeyDown(previousCameraViewKey)) {
                currentOffsetIndex = currentOffsetIndex - 1 >= 0 ? currentOffsetIndex - 1 : targetOffsets.Length - 1;
            }

            if (Input.GetKeyDown(nextCameraViewKey)) {
                currentOffsetIndex = currentOffsetIndex + 1 < targetOffsets.Length ? currentOffsetIndex + 1 : 0;
            }

            Vector3 move = (target.Position + targetOffsets[currentOffsetIndex].Position + obstacleAvoidanceModule.CurrentAvoidanceOffset) - transform.position;
            if (move.magnitude > FOLLOW_DISTANCE_THRESHOLD) {
                transform.position = transform.position + move * movementSpeedMultiplier * deltaTime;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientations[currentOffsetIndex], rotationSpeed * deltaTime);
        }

        public override void FixedUpdate(float deltaTime) {
            base.FixedUpdate(deltaTime);
            obstacleAvoidanceModule.Update(deltaTime);
        }
    }
}
