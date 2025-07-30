using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] private int iterations = 10;
    [SerializeField] private int walkLenth = 10;
    [SerializeField] private bool startRandomlyEachIteration = true;

    public void RunProceduralGeneration()
    {
        var floorPositions = CreateFloors();
        foreach (var pos in floorPositions)
        {
            Debug.Log(string.Format("{0},{1}", pos.x, pos.y));
        }
    }

    private HashSet<Vector2Int> CreateFloors()
    {
        HashSet<Vector2Int> floorPositions = ProceduralGeneration.SimpleRandomWalk(startPosition, walkLenth);
        Vector2Int iterationStarPosition = startPosition;
        for (int i = 0; i < iterations; ++i)
        {
            floorPositions.UnionWith(ProceduralGeneration.SimpleRandomWalk(iterationStarPosition, walkLenth));
            if (startRandomlyEachIteration)
            {
                iterationStarPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }

}
