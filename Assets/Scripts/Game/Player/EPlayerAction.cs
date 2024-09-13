
namespace Game.Player
{
    public enum EPlayerAction
    {
        Jump,
        Interact,
        PreviousCameraView,
        NextCameraView
    }

    public static class PlayerActionHelper
    {
        public static string ActionToString(EPlayerAction action) {
            switch (action)
            {
                case EPlayerAction.PreviousCameraView:
                    return "Rotate camera view";
                case EPlayerAction.NextCameraView:
                    return "Rotate camera view";
                default:
                    return action.ToString();
            }
        }
    }
}

