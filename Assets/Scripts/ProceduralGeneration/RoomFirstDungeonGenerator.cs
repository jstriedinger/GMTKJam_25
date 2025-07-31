using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDungeonGenerator : CorridorDungeonGenerator
{
    [SerializeField] private int minRoomWidth = 4;
    [SerializeField] private int minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20;
    [SerializeField] private int dungeonHeight = 20;
    [SerializeField][Range(0, 10)] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralGeneration.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomList);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add(Vector2Int.RoundToInt(room.center));
        }

        List<List<Vector2Int>> corridors = ConnectRooms(roomCenters, floor);
        if (widenCorridors)
        {
            WidenCorridors(floor, corridors);
        }

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; ++i)
        {
            var roomBounds = roomList[i];
            var roomCenter = Vector2Int.RoundToInt(roomBounds.center);
            var roomFloor = RunRandomWalk(randomWalkParams, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x < (roomBounds.xMin + offset) || position.x > (roomBounds.xMax - offset) ||
                    position.y < (roomBounds.yMin + offset) || position.y > (roomBounds.yMax - offset))
                {
                    continue;
                }

                floor.Add(position);
            }
        }

        return floor;
    }

    private List<List<Vector2Int>> ConnectRooms(List<Vector2Int> roomCenters, HashSet<Vector2Int> floorPositions)
    {
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            List<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            corridors.Add(newCorridor);
            floorPositions.UnionWith(newCorridor);
            currentRoomCenter = closest;
        }

        return corridors;
    }

    private List<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        float minDistance = float.MaxValue;
        Vector2Int closestRoomCenter = currentRoomCenter;
        foreach (var roomCenter in roomCenters)
        {
            float distance = Vector2Int.Distance(currentRoomCenter, roomCenter);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoomCenter = roomCenter;
            }
        }

        return closestRoomCenter;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    floor.Add((Vector2Int)room.min + new Vector2Int(col, row));
                }
            }
        }

        return floor;
    }
}
