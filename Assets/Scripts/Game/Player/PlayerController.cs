using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.States;

namespace Game.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IUpdatable
    {
        #region StateMachine

        private StateMachine stateMachine;
        private PlayerIdlState idlState;
        private PlayerWalkingState walkingState;
        private PlayerJumpingState jumpingState;

        #endregion

        public int Hash => hash;

        private int hash;

        public void Awake()
        {
            hash = gameObject.GetHashCode();

            Animator animator = GetComponent<Animator>();

            stateMachine = new StateMachine();
            idlState = new PlayerIdlState(0, StateDefinitions.Player.Idl, stateMachine, animator);
            walkingState = new PlayerWalkingState(1, StateDefinitions.Player.Walking, stateMachine, animator);
            jumpingState = new PlayerJumpingState(2, StateDefinitions.Player.Jumping, stateMachine, animator);

            stateMachine.AddState(idlState);
            stateMachine.AddState(walkingState);
            stateMachine.AddState(jumpingState);

            stateMachine.Initialize();
        }

        private void OnEnable()
        {
            GameManager.Instance.GameUpdater.AddUpdatable(this);
        }

        private void OnDisable()
        {
            GameManager.Instance.GameUpdater.RemoveUpdatable(this);
        }

        public void FrameUpdate(float deltaTime)
        {
            stateMachine.Update(deltaTime);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
        }
#endif
    }
}

