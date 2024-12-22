using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const float GRID_POINT_DISTANCE_OFFSET = 1f;

    public enum GridPointState
    {
        Empty,
        Full,
        Obstacle,
        Breakable
    }
}
