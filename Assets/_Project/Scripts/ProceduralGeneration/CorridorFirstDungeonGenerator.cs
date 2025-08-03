using System;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : CorridorDungeonGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField][Range(0.1f, 1)] private float roomPercent = 0.8f;

    protected override void RunProceduralGeneration(bool ranFromEditor)
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        var corridors = CreateCorridors(floorPositions, potentialRoomPositions);
        CreateRooms(floorPositions, potentialRoomPositions);
        if (generationParams.widenCorridors)
        {
            WidenCorridors(floorPositions, corridors);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateRooms(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        foreach (var position in potentialRoomPositions)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < roomPercent || IsDeadEnd(floorPositions, position))
            {
                floorPositions.UnionWith(RunRandomWalk(generationParams.roomParams, position));
            }
        }
    }

    private bool IsDeadEnd(HashSet<Vector2Int> floorPositions, Vector2Int position)
    {
        int connections = 0;
        foreach (var direction in ProceduralGeneration.Direction2D.CardinalDirectionsList)
        {
            if (floorPositions.Contains(position + direction))
            {
                ++connections;
            }
        }

        return !(connections > 1);
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; ++i)
        {
            var corridor = ProceduralGeneration.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }

        return corridors;
    }
}
