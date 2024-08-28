using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateDefinitions
{
    public static class GameState
    {
        public static string Start => "GameStartState";
        public static string Level => "GameLevelState";
        public static string End => "GameEndState";
    }

    public static class Player
    {
        public static string Idl => "Idl";
        public static string Walking => "Walking";
        public static string Jumping => "Jumping";
    }
}
