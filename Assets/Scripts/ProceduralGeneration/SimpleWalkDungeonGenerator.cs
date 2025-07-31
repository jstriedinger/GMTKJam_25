using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkData randomWalkParams;

    protected override void RunProceduralGeneration()
    {
        var floorPositions = RunRandomWalk(randomWalkParams, startPosition);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions,tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData parameters, Vector2Int roomStartPosition)
    {
        HashSet<Vector2Int> floorPositions = ProceduralGeneration.SimpleRandomWalk(roomStartPosition, parameters.walkLenth);
        Vector2Int iterationStarPosition = roomStartPosition;
        for (int i = 0; i < parameters.iterations; ++i)
        {
            floorPositions.UnionWith(ProceduralGeneration.SimpleRandomWalk(iterationStarPosition, parameters.walkLenth));
            if (parameters.startRandomlyEachIteration)
            {
                iterationStarPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }

}
