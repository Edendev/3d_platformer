using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game.States
{
    public class StateMachine : IDisposable
    {
        public State CurrentState => currentState;
        public State PreviousState => previousState; 
        public State NextState => nextState;

        private Dictionary<uint, State> statesByID = new Dictionary<uint, State>();
        private Dictionary<string, State> statesByName = new Dictionary<string, State>();
        private State currentState;
        private State previousState;
        private State nextState;

        public StateMachine() { }

        public void Initialize() {
            if (statesByID.Count == 0) return;
            if (!statesByID.ContainsKey(0)) return;
            ChangeState(0);
        }

        public void Update(float deltaTime) {
            if (currentState == null) return;
            currentState.Update(deltaTime);
        }

        public void PhysicsUpdate() {
            if (currentState == null) return;
            currentState.PhysicsUpdate();
        }

        public void LateUpdate() {
            if (currentState == null) return;
            currentState.LateUpdate();
        }

        public bool ChangeState(uint stateID) {
            if (!statesByID.ContainsKey(stateID)) return false;
            if (currentState != null && currentState.ID == stateID) return false;
            previousState = currentState;
            nextState = statesByID[stateID];
            if (previousState != null) previousState.Exit();
            currentState = statesByID[stateID];
            currentState.Enter();
            return true;
        }

        public bool ChangeState(string stateName) {
            if (!statesByName.ContainsKey(stateName)) return false;
            if (currentState != null && currentState.Name == stateName) return false;
            previousState = currentState;
            nextState = statesByName[stateName];
            if (previousState != null) previousState.Exit();
            currentState = statesByName[stateName];
            currentState.Enter();
            return true;
        }

        public void ExitMachine() {
            previousState = currentState;
            nextState = null;
            if (currentState != null) currentState.Exit();
            currentState = null;
        }

        public bool GetStateByID(uint id, out State state) {
            return statesByID.TryGetValue(id, out state);
        }

        public bool GetStateByName(string name, out State state) {
            return statesByName.TryGetValue(name, out state);
        }

        public bool GetStateByName<T>(string name, out T state) 
            where T : State 
        {
            bool success = statesByName.TryGetValue(name, out State _state);
            state = success ? (T)_state : null;
            return success;
        }

        public void AddState(State state) {
            bool addedToID = statesByID.TryAdd(state.ID, state);
            if (!statesByName.TryAdd(state.Name, state) && addedToID)
            {
#if UNITY_EDITOR
                Debug.Log("States with different ID but same name are not allowed in the same state machine");
#endif
            }
        }

        public void RemoveState(State state) {
            statesByID.Remove(state.ID);
            statesByName.Remove(state.Name);
        }

        public void Dispose() {
            foreach (State state in statesByID.Values) {
                state.Dispose();
            }
            statesByID.Clear();
            statesByID.Clear();
            currentState = null;
            previousState = null;
        }
    }
}