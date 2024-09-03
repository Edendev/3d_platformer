using System;
using Game.PhysicsSystem;
using Game.Settings;
using Game.States;
using Game.Interfaces;
using Game.Interaction;
using Game.Tween;
using Game.Player;
using UnityEngine;

namespace Game.Systems
{
    /// <summary>
    /// Handles the game logic and the spawning/despawning of the player. Follows a state machine pattern/
    /// </summary>
    public class PlayerSystem : ISystem, IPosition
    {
        public ESystemAccessType AccessType => ESystemAccessType.Private;
        public Vector3 Position => playerGO.transform.position;

        private readonly int hash;

        // State machine
        private readonly StateMachine stateMachine;
        private readonly PlayerIdlState idlState;
        private readonly PlayerWalkingState walkingState;
        private readonly PlayerJumpingState jumpingState;

        // Modules
        private readonly PhysicsModule physicsModule;
        private readonly TweenableModule tweenableModule;
        private readonly PlayerDeathModule deathModule;
        private readonly InteractionModule interactionModule;

        private readonly UpdateSystem updateSystem;
        private GameObject playerGO;

        public PlayerSystem(UpdateSystem updateSystem, GameSettingsSO gameSettingsSO, SettingsSystem settingsSystem, Transform cameraTransform)
        {
            this.updateSystem = updateSystem;
            SystemHash.TryGetHash(typeof(PlayerSystem), out hash);

            playerGO = GameObject.Instantiate(gameSettingsSO.PlayerSO.PlayerGO);
            playerGO.SetActive(false);

            Animator animator = playerGO.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"{nameof(PlayerSystem)} player is missing an Animator component");
                return;
            }

            TriggerEventsAnnouncer triggerEventsAnnouncer = playerGO.GetComponent<TriggerEventsAnnouncer>();
            if (triggerEventsAnnouncer == null)
            {
                Debug.LogError($"{nameof(PlayerSystem)} player is missing an TriggerEventsAnnouncer component");
                return;
            }

            CollisionEventsAnnouncer collisionEventsAnnouncer = playerGO.GetComponent<CollisionEventsAnnouncer>();
            if (collisionEventsAnnouncer == null)
            {
                Debug.LogError($"{nameof(PlayerSystem)} player is missing an CollisionEventsAnnouncer component");
                return;
            }

            physicsModule = new PhysicsModule(gameSettingsSO.MaxGravitySpeed, gameSettingsSO.GravityConstant, playerGO.transform);
            tweenableModule = new TweenableModule(physicsModule, playerGO.transform);
            deathModule = new PlayerDeathModule(triggerEventsAnnouncer, collisionEventsAnnouncer);
            interactionModule = new InteractionModule(playerGO, gameSettingsSO.PlayerSO.InteractionRange, settingsSystem);

            stateMachine = new StateMachine();
            idlState = new PlayerIdlState(0, StateDefinitions.Player.Idl, stateMachine, animator, interactionModule, physicsModule, settingsSystem);
            walkingState = new PlayerWalkingState(1, StateDefinitions.Player.Walking, stateMachine, animator, interactionModule, physicsModule, gameSettingsSO.PlayerSO, cameraTransform, settingsSystem);
            jumpingState = new PlayerJumpingState(2, StateDefinitions.Player.Jumping, stateMachine, animator, interactionModule, physicsModule, gameSettingsSO.PlayerSO, cameraTransform, settingsSystem);

            stateMachine.AddState(idlState);
            stateMachine.AddState(walkingState);
            stateMachine.AddState(jumpingState);

            stateMachine.Initialize();
        }

        public void Destroy() {
            stateMachine.Dispose();
            tweenableModule?.Dispose();
            deathModule?.Dispose();
            updateSystem?.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
            updateSystem?.RemoveUpdatable(EUpdateTime.FixUpdate, hash);
        }

        public void SubscribeToDeathEvent(Action listener) => deathModule.onDeath += listener;
        public void UnsubscribeTFromeathEvent(Action listener) => deathModule.onDeath -= listener;

        public void SpawnPlayer(Vector3 position) {
            playerGO.transform.position = position;
            playerGO.transform.rotation = Quaternion.identity;
            playerGO.SetActive(true);
            updateSystem.AddUpdatable(EUpdateTime.FrameUpdate, hash, FrameUpdate);
            updateSystem.AddUpdatable(EUpdateTime.FixUpdate, hash, FixUpdate);
            updateSystem.AddUpdatable(EUpdateTime.LateUpdate, hash, LateUpdate);
        }

        public void DespawnPlayer() {
            playerGO.SetActive(false);
            updateSystem.RemoveUpdatable(EUpdateTime.FrameUpdate, hash);
            updateSystem.RemoveUpdatable(EUpdateTime.FixUpdate, hash);
            updateSystem.RemoveUpdatable(EUpdateTime.LateUpdate, hash);
        }

        public void FrameUpdate(float deltaTime) {
            stateMachine.Update(deltaTime);
        }

        public void FixUpdate(float deltaTime) {
            physicsModule.Update(deltaTime);
            tweenableModule.Update(deltaTime);
            stateMachine.FixedUpdate(deltaTime);
        }

        public void LateUpdate(float deltaTime) {
            stateMachine.LateUpdate(deltaTime);
        }
    }
}
