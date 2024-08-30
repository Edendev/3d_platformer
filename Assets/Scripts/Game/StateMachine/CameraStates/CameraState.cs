using Game.PhysicsSystem;
using Game.Settings;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CameraControl;

namespace Game.States
{
    public abstract class CameraState : State
    {
        protected readonly Transform transform;

        public CameraState(uint id, string name, StateMachine stateMachine, Transform transform) : base(id, name, stateMachine)
        {
            this.transform = transform;
        }
    }
}
