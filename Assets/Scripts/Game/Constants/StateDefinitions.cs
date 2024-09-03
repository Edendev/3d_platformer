public static class StateDefinitions
{
    public static class GameState
    {
        public static string Start => "GameStartState";
        public static string Level => "GameLevelState";
        public static string LevelCompleted => "GameLevelCompletedState";
        public static string GameOver => "GameOverState";
    }

    public static class Player
    {
        public static string Idl => "Idl";
        public static string Walking => "Walking";
        public static string Jumping => "Jumping";
    }

    public static class  Camera
    {
        public static string FollowTarget => "FollowTarget";
        public static string UI => "UI";
    }
}
