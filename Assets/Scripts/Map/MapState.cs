using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    Moving,
    Filled,
    Rotating,
    Explode
}
public static class MapState
{
    public static GameState GameStateInfo;
}
