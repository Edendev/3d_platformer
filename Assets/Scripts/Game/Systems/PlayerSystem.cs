using Game.PhysicsSystem;
using Game.Settings;
using Game.States;
using Game.Interfaces;
using Game.Interaction;
using UnityEngine;
using Game.Tween;
using System;
using UnityEditor;

namespace Game.Systems
{
    public class PlayerSystem : ISystem, IPosition
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;

        private readonly StateMachine stateMachine;
        private readonly PlayerIdlState idlState;
        private readonly PlayerWalkingState walkingState;
        private readonly PlayerJumpingState jumpingState;

        private readonly int hash;

        private readonly PhysicsModule physicsModule;
        private readonly TweenableModule tweenableModule;
        private readonly PlayerDeathModule deathModule;
        private readonly InteractionModule interactionModule;

        private readonly UpdateSystem updateSystem;
        private GameObject playerGO;

        public PlayerSystem(UpdateSystem updateSystem, GameSettingsSO gameSettingsSO, Transform cameraTransform)
        {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(PlayerSystem), out hash);

            playerGO = GameObject.Instantiate(gameSettingsSO.PlayerSO.PlayerGO);
            playerGO.SetActive(false);

            Animator animator = playerGO.GetComponent<Animator>();
            if (animator == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(PlayerSystem)} player is missing an Animator component");
#endif
            }

            TriggerEventsAnnouncer triggerEventsAnnouncer = playerGO.GetComponent<TriggerEventsAnnouncer>();
            if (triggerEventsAnnouncer == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{nameof(PlayerSystem)} player is missing an TriggerEventsAnnouncer component");
#endif
            }

            CollisionEventsAnnouncer collisionEventsAnnouncer = playerGO.GetComponent<CollisionEventsAnnouncer>();
            if (collisionEventsAnnouncer == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{nameof(PlayerSystem)} player is missing an CollisionEventsAnnouncer component");
#endif
            }

            physicsModule = new PhysicsModule(gameSettingsSO.MaxGravitySpeed, gameSettingsSO.GravityConstant, playerGO.transform);
            tweenableModule = new TweenableModule(physicsModule, playerGO.transform);
            deathModule = new PlayerDeathModule(triggerEventsAnnouncer, collisionEventsAnnouncer);
            interactionModule = new InteractionModule(playerGO, gameSettingsSO.PlayerSO.InteractionRange);

            stateMachine = new StateMachine();
            idlState = new PlayerIdlState(0, StateDefinitions.Player.Idl, stateMachine, animator, physicsModule);
            walkingState = new PlayerWalkingState(1, StateDefinitions.Player.Walking, stateMachine, animator, physicsModule, gameSettingsSO.PlayerSO, cameraTransform);
            jumpingState = new PlayerJumpingState(2, StateDefinitions.Player.Jumping, stateMachine, animator, physicsModule, gameSettingsSO.PlayerSO, cameraTransform);

            stateMachine.AddState(idlState);
            stateMachine.AddState(walkingState);
            stateMachine.AddState(jumpingState);

            stateMachine.Initialize();
        }

        public void Destroy() {
            stateMachine.Dispose();
            updateSystem?.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
            updateSystem?.RemoveUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash);
            tweenableModule?.Dispose();
            deathModule?.Dispose();
        }

        public void SpawnPlayer(Vector3 position)
        {
            playerGO.transform.position = position;
            playerGO.transform.rotation = Quaternion.identity;
            playerGO.SetActive(true);

            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash, FrameUpdate);
            updateSystem.AddUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash, FixUpdate);
        }

        public void DespawnPlayer()
        {
            playerGO.SetActive(false);

            updateSystem.RemoveUpdatable(UpdateSystem.EUpdateTime.FrameUpdate, hash);
            updateSystem.RemoveUpdatable(UpdateSystem.EUpdateTime.FixUpdate, hash);
        }

        public void SubscribeToDeathEvent(Action listener) => deathModule.onDeath += listener;
        public void UnsubscribeTFromeathEvent(Action listener) => deathModule.onDeath -= listener;

        public void FrameUpdate(float deltaTime)
        {
            interactionModule.Update(deltaTime);
            stateMachine.Update(deltaTime);
        }

        public void FixUpdate(float deltaTime)
        {
            physicsModule.Update(deltaTime);
            tweenableModule.Update(deltaTime);
            stateMachine.FixedUpdate(deltaTime);
        }

        public Vector3 Position => playerGO.transform.position;
    }
}
