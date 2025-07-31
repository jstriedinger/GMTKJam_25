using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var wallPositions = FindWallsInDirections(floorPositions, ProceduralGeneration.Direction2D.CardinalDirectionsList);
        foreach (var wallPosition in wallPositions)
        {
            tilemapVisualizer.PaintBasicWallTile(wallPosition);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach(var direction in directions)
            {
                if (!floorPositions.Contains(position + direction))
                {
                    // Found a wall
                    wallPositions.Add(position + direction);
                }
            }
        }

        return wallPositions;
    }
}
