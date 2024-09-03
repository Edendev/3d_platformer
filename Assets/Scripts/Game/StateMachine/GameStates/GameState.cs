using Game.Systems;
using UnityEngine;

namespace Game.States
{
    public abstract class GameState : State
    {
        protected readonly CameraControlSystem camera;

        public GameState(uint id, string name, StateMachine stateMachine, CameraControlSystem camera) : base(id, name, stateMachine)
        {
            this.camera = camera;
        }

        public override void Update(float deltaTime) { 
            base.Update(deltaTime);
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
    }
}
