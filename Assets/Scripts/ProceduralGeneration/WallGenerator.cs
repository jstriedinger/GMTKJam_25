using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, AbstractTilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, ProceduralGeneration.Direction2D.CardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, ProceduralGeneration.Direction2D.DiagonalDirectionsList);

        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWall(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateBasicWall(AbstractTilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var wallPosition in wallPositions)
        {
            int neighbors = 0;
            var directions = ProceduralGeneration.Direction2D.CardinalDirectionsList;
            for (int i = 0; i < directions.Count; ++i)
            {
                var neighborPos = wallPosition + directions[i];
                if (floorPositions.Contains(neighborPos))
                {
                    neighbors |= 1 << directions.Count - 1 - i;
                }
            }
            tilemapVisualizer.PaintBasicWallTile(wallPosition, neighbors);
        }
    }

    private static void CreateCornerWall(AbstractTilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var wallPosition in wallPositions)
        {
            int neighbors = 0;
            var directions = ProceduralGeneration.Direction2D.EightDirectionsList;
            for (int i = 0; i < directions.Count; ++i)
            {
                var neighborPos = wallPosition + directions[i];
                if (floorPositions.Contains(neighborPos))
                {
                    neighbors |= 1 << directions.Count - 1 - i;
                }
            }
            tilemapVisualizer.PaintCornerWallTile(wallPosition, neighbors);
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
