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
            previousPostition = previousPostition + GetRandomCardinalDirection();
            path.Add(previousPostition);
        }

        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>() { startPosition };
        var direction = GetRandomCardinalDirection();
        var currentPosition = startPosition;

        for (int i = 0; i < corridorLength; ++i)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }

    private static Vector2Int GetRandomCardinalDirection()
    {
        int directionIdx = Random.Range(0, Direction2D.CardinalDirectionsList.Count);
        return Direction2D.CardinalDirectionsList[directionIdx];
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while (roomsQueue.Count != 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.x >= minWidth && room.size.y >= minHeight)
            {
                bool splitHorizonatallyFirst = Random.value < 0.5;
                bool couldSplit = TrySplit(splitHorizonatallyFirst, minWidth, minHeight, roomsQueue, room) ||
                    TrySplit(!splitHorizonatallyFirst, minWidth, minHeight, roomsQueue, room);


                if (!couldSplit)
                {
                    roomList.Add(room);
                }
            }
        }

        return roomList;
    }

    private static bool TrySplit(bool splitHorizontally, int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        if (splitHorizontally)
        {
            return TrySplitHorizontally(minHeight, roomsQueue, room);
        }

        return TrySplitVertically(minWidth, roomsQueue, room);
    }

    private static bool TrySplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        if (room.size.y < minHeight * 2)
        {
            return false;
        }

        SplitHorizontally(roomsQueue, room);

        return true;
    }

    private static bool TrySplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        if (room.size.x < minWidth * 2)
        {
            return false;
        }

        SplitVertically(roomsQueue, room);

        return true;
    }

    private static void SplitVertically(Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int splitPoint = Random.Range(1, room.size.x);
        roomsQueue.Enqueue(new BoundsInt(room.min, new Vector3Int(splitPoint, room.size.y, room.size.z)));
        roomsQueue.Enqueue(new BoundsInt(
            new Vector3Int(room.xMin + splitPoint, room.yMin, room.zMin),
            new Vector3Int(room.size.x - splitPoint, room.size.y, room.size.z)));

    }

    private static void SplitHorizontally(Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int splitPoint = Random.Range(1, room.size.y);
        roomsQueue.Enqueue(new BoundsInt(room.min, new Vector3Int(room.size.x, splitPoint, room.size.z)));
        roomsQueue.Enqueue(new BoundsInt(
            new Vector3Int(room.xMin, room.yMin + splitPoint, room.zMin),
            new Vector3Int(room.size.x, room.size.y - splitPoint, room.size.z)));
    }

    public class Direction2D
    {
        public static List<Vector2Int> CardinalDirectionsList = new List<Vector2Int>() {
            new Vector2Int(0, 1), // NORTH
            new Vector2Int(1, 0), // EAST
            new Vector2Int(0, -1), // SOUTH
            new Vector2Int(-1, 0)  // WEST
        };

        public static List<Vector2Int> DiagonalDirectionsList = new List<Vector2Int>() {
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1)
        };

        public static List<Vector2Int> EightDirectionsList = new List<Vector2Int>() {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1)
        };
    }

}
