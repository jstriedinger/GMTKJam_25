using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGeneration
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        Vector2Int previousPostition = startPosition;

        for (int i = 0; i < walkLength; ++i)
        {
            previousPostition = startPosition + GetRandomCardinalDirection();
            path.Add(previousPostition);
        }

        return path;
    }

    private static Vector2Int GetRandomCardinalDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                return new Vector2Int(0, 1);
            case 1:
                return new Vector2Int(0, -1);
            case 2:
                return new Vector2Int(1, 0);
            case 3:
                return new Vector2Int(-1, 0);
        }

        return Vector2Int.zero;
    }

}
