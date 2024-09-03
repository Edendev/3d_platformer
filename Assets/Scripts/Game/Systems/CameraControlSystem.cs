using Game.Settings;
using UnityEngine;
using Game.States;
using Game.CameraControl;
using Game.Interfaces;

namespace Game.Systems
{
    /// <summary>
    /// Handle the logic of the camera. Follows a state machine pattern.
    /// </summary>
    public class CameraControlSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        public Transform CameraTransform => camera.transform;

        private readonly int hash;

        // State machine
        private readonly StateMachine stateMachine;
        private readonly CameraFollowTargetState followTargetState;
        private readonly CameraUIState UIState;
        
        // Modules
        private readonly ObstacleAvoidanceModule obstacleAvoidanceModule;
        
        private readonly UpdateSystem updateSystem;        
        private readonly Camera camera;

        public CameraControlSystem(GameSettingsSO gameSettingsSO, UpdateSystem updateSystem, SettingsSystem settingsSystem)
        {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(CameraControlSystem), out hash);

            Vector3 cameraUIPosition = settingsSystem.GetCameraUIPosition(GameManager.Instance.CurrentLevelId);
            Vector3 cameraUIRotation = settingsSystem.GetCameraUIRotation(GameManager.Instance.CurrentLevelId);

            GameObject cameraGO = GameObject.Instantiate(
                gameSettingsSO.CameraSO.CameraGO,
                cameraUIPosition,
                Quaternion.Euler(cameraUIRotation)
            );

            camera = cameraGO.GetComponent<Camera>();
            if (camera == null)
            {
                Debug.LogError($"{nameof(CameraControlSystem)} is missing a Camera component.");
                return;
            }

            obstacleAvoidanceModule = new ObstacleAvoidanceModule(
                gameSettingsSO.CameraSO.ObstacleAvoidanceRadius,
                gameSettingsSO.CameraSO.ObstacleAvoidanceSpeed,
                LayerMask.GetMask("Default"),
                camera.transform
            );

            stateMachine = new StateMachine();
            UIState = new CameraUIState(0, StateDefinitions.Camera.UI, stateMachine, camera.transform, gameSettingsSO, cameraUIPosition, cameraUIRotation);
            followTargetState = new CameraFollowTargetState(1, StateDefinitions.Camera.FollowTarget, stateMachine, camera.transform, obstacleAvoidanceModule, gameSettingsSO.CameraSO, settingsSystem);

            stateMachine.AddState(followTargetState);
            stateMachine.AddState(UIState);

            stateMachine.Initialize();

            updateSystem.AddUpdatable(EUpdateTime.FrameUpdate, hash, FrameUpdate);
            updateSystem.AddUpdatable(EUpdateTime.FixUpdate, hash, FixedUpdate);
            updateSystem.AddUpdatable(EUpdateTime.LateUpdate, hash, LateUpdate);
        }

        public void Destroy() {
            stateMachine.Dispose();
            updateSystem?.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
            updateSystem?.RemoveUpdatable(EUpdateTime.FixUpdate, hash);
            updateSystem?.RemoveUpdatable(EUpdateTime.LateUpdate, hash);
        }
        
        public void SetTarget(IPosition target) {
            followTargetState.SetTarget(target);
        }

        public void ChangeState(string newState) {
            stateMachine.ChangeState(newState);
        }
        
        private void FrameUpdate(float deltaTime) {
            stateMachine.Update(deltaTime);
        }

        private void FixedUpdate(float deltaTime) {
            stateMachine.FixedUpdate(deltaTime);
        }

        private void LateUpdate(float deltaTime) {
            stateMachine.LateUpdate(deltaTime);
        }
    }
}
