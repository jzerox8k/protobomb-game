using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
    None = 0,
    North = 1<<0,
    East = 1<<1,
    South = 1<<2,
    West = 1<<3
}


public static class DirectionPrimitives
{
    public static Dictionary<Direction, Vector2> DirectionToVector = new Dictionary<Direction, Vector2>
    {
        { Direction.None, Vector2.zero },
        { Direction.North, Vector2.up },
        { Direction.East, Vector2.right },
        { Direction.South, Vector2.down },
        { Direction.West, Vector2.left }
    };
}


