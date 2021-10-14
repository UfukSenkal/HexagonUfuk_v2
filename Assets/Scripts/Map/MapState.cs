using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonDemo
{

    public enum GameState
    {
        Moving,
        Filled,
        Rotating,
        Explode,
        Rotated,
        Match
    }
    public static class MapState
    {
        public static GameState GameStateInfo;
    }
}