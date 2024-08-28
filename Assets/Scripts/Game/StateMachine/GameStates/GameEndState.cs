using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class GameEndState : State
    {
        public GameEndState(uint id, string name, StateMachine stateMachine) : base(id, name, stateMachine) { }
        public override System.Type GetType() => typeof(GameEndState);
    }
}
