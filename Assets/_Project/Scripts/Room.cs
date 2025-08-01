using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public BoundsInt bounds;
    public Vector2Int center;
    public HashSet<Vector2Int> floorPositions;

    public Room(BoundsInt bounds, HashSet<Vector2Int> floorPositions)
    {
        this.bounds = bounds;
        center = Vector2Int.RoundToInt(bounds.center);
        this.floorPositions = floorPositions;
    }
}
