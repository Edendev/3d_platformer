using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game.States
{
    /// <summary>
    /// Finite state machine implementation.
    /// </summary>
    public class StateMachine : IDisposable
    {
        private Dictionary<uint, State> statesByID = new Dictionary<uint, State>();
        private Dictionary<string, State> statesByName = new Dictionary<string, State>();
        private State currentState;

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

        public void FixedUpdate(float deltaTime) {
            if (currentState == null) return;
            currentState.FixedUpdate(deltaTime);
        }

        public void LateUpdate(float deltaTime) {
            if (currentState == null) return;
            currentState.LateUpdate(deltaTime);
        }

        public bool ChangeState(uint stateID) {
            if (!statesByID.ContainsKey(stateID)) return false;
            if (currentState != null && currentState.ID == stateID) return false;
            State previousState = currentState;
            if (previousState != null) previousState.Exit();
            currentState = statesByID[stateID];
            currentState.Enter();
            return true;
        }

        public bool ChangeState(string stateName) {
            if (!statesByName.ContainsKey(stateName)) return false;
            if (currentState != null && currentState.Name == stateName) return false;
            State previousState = currentState;
            if (previousState != null) previousState.Exit();
            currentState = statesByName[stateName];
            currentState.Enter();
            return true;
        }

        public void ExitMachine() {
            if (currentState != null) currentState.Exit();
            currentState = null;
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
        }
    }
}