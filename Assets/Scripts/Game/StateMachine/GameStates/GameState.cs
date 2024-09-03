using Game.Systems;

namespace Game.States
{
    public abstract class GameState : State
    {
        protected readonly CameraControlSystem camera;

        public GameState(uint id, string name, StateMachine stateMachine, CameraControlSystem camera) : base(id, name, stateMachine)
        {
            this.camera = camera;
        }
    }
}
