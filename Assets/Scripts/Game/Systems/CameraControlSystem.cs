using Game.Settings;
using UnityEngine;
using Game.States;
using Game.CameraControl;
using Game.Interfaces;

namespace Game.Systems
{
    public class CameraControlSystem : ISystem
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;
        public Transform CameraTransform => camera.transform;

        private readonly StateMachine stateMachine;
        private readonly CameraFollowTargetState followTargetState;
        private readonly CameraUIState UIState;

        private readonly int hash;
        
        private readonly ObstacleAvoidanceModule obstacleAvoidanceModule;
        private readonly UpdateSystem updateSystem;
        private readonly Camera camera;

        public CameraControlSystem(GameSettingsSO gameSettingsSO, UpdateSystem updateSystem)
        {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(CameraControlSystem), out hash);

            GameObject cameraGO = GameObject.Instantiate(
                gameSettingsSO.CameraSO.CameraGO,
                gameSettingsSO.CameraUIPosition,
                Quaternion.Euler(gameSettingsSO.CameraUIRotation)
            );

            camera = cameraGO.GetComponent<Camera>();
            if (camera == null)
            {
                throw new System.Exception($"{nameof(CameraControlSystem)} is missing a Camera component.");
            }

            obstacleAvoidanceModule = new ObstacleAvoidanceModule(
                gameSettingsSO.CameraSO.ObstacleAvoidanceRadius,
                gameSettingsSO.CameraSO.ObstacleAvoidanceSpeed,
                camera.transform
            );

            stateMachine = new StateMachine();
            UIState = new CameraUIState(0, StateDefinitions.Camera.UI, stateMachine, camera.transform, gameSettingsSO);
            followTargetState = new CameraFollowTargetState(1, StateDefinitions.Camera.FollowTarget, stateMachine, camera.transform, obstacleAvoidanceModule, gameSettingsSO.CameraSO);

            stateMachine.AddState(followTargetState);
            stateMachine.AddState(UIState);

            stateMachine.Initialize();

            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash, FrameUpdate);
            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash, FixedUpdate);
        }

        public void Destroy()
        {
            stateMachine.Dispose();
            updateSystem?.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
            updateSystem?.RemoveUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash);
        }
        
        public void SetTarget(IPosition target)
        {
            followTargetState.SetTarget(target);
        }

        public void ChangeState(string newState)
        {
            stateMachine.ChangeState(newState);
        }
        
        private void FrameUpdate(float deltaTime)
        {
            stateMachine.Update(deltaTime);
        }

        private void FixedUpdate(float deltaTime)
        {
            stateMachine.FixedUpdate(deltaTime);
        }
    }
}
