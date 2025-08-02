using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDungeonGenerator : CorridorDungeonGenerator
{
    [SerializeField] PropsManager propsManager;
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private GameObject goalIndicator;

    // Fill holes that are fully surrounded, or surrounded by at least 3 floor tiles
    private static HashSet<int> shouldFillHole = new HashSet<int>
    {
        0b0111,
        0b1011,
        0b1101,
        0b1110,
        0b1111,
    };

    protected override void RunProceduralGeneration()
    {
        Dungeon dungeonData = FindFirstObjectByType<Dungeon>();
        if (dungeonData == null)
        {
            Debug.LogError("Add a Dungeon Object to the scene.");
            return;
        }

        dungeonData.Clear();

        int tries = 0;
        while (tries < generationParams.maxTries)
        {
            if (CreateRooms(dungeonData))
            {
                break;
            }

            ++tries;
        }

        if (tries >= generationParams.maxTries)
        {
            CreateRooms(dungeonData, true);
        }

        Debug.Log(string.Format("Tries: {0}", tries));
    }

    private bool CreateRooms(Dungeon dungeonData, bool ignoreConstraints = false)
    {
        var roomList = ProceduralGeneration.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(generationParams.dungeonWidth, generationParams.dungeonHeight, 0)), generationParams.minRoomWidth, generationParams.minRoomHeight);

        if (!ignoreConstraints)
        {
            if (roomList.Count < generationParams.minRooms)
            {
                return false;
            }
        }

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        List<Room> rooms = new List<Room>();
        if (generationParams.randomWalkRooms)
        {
            rooms = CreateRoomsRandomly(roomList);
        }
        else
        {
            rooms = CreateSimpleRooms(roomList);
        }

        foreach(var room in rooms)
        {
            floor.UnionWith(room.floorPositions);
        }

        FindAndSpawnStartAndGoal(dungeonData, rooms);

        if (generationParams.generateCorridors)
        {
            List<List<Vector2Int>> corridors = ConnectRooms(rooms, floor);
            if (generationParams.widenCorridors)
            {
                WidenCorridors(floor, corridors);
            }
        }

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        dungeonData.rooms = rooms;

        propsManager.ProcessRooms();

        return true;
    }

    private void FindAndSpawnStartAndGoal(Dungeon dungeonData, List<Room> rooms)
    {
        Vector2Int startRoomCenter = rooms[0].center;
        for (int i = 1; i < rooms.Count; ++i)
        {
            if (rooms[i].center.x < startRoomCenter.x)
            {
                startRoomCenter = rooms[i].center;
            }
        }

        float maxDistance = 0;
        Vector2Int goalRoomCenter = startRoomCenter;
        for (int i = 0; i < rooms.Count; ++i)
        {
            float distance = Vector2Int.Distance(rooms[i].center, startRoomCenter);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                goalRoomCenter = rooms[i].center;
            }
        }

        dungeonData.spawnPosition = new Vector3Int(startRoomCenter.x, 0, startRoomCenter.y);

        Instantiate(spawnIndicator, dungeonData.spawnPosition, Quaternion.identity, levelParent);
        Instantiate(goalIndicator, new Vector3Int(goalRoomCenter.x, 0, goalRoomCenter.y), Quaternion.identity, levelParent);
    }

    private List<Room> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        List<Room> rooms = new List<Room>();
        for (int i = 0; i < roomList.Count; ++i)
        {
            HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
            var roomBounds = roomList[i];
            var roomCenter = Vector2Int.RoundToInt(roomBounds.center);
            var roomFloor = RunRandomWalk(generationParams.roomParams, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x < (roomBounds.xMin + generationParams.offset) || position.x > (roomBounds.xMax - generationParams.offset) ||
                    position.y < (roomBounds.yMin + generationParams.offset) || position.y > (roomBounds.yMax - generationParams.offset))
                {
                    continue;
                }

                floor.Add(position);
            }

            if (generationParams.fillHoles)
            {
                int passes = 3;
                while (passes > 0)
                {
                    FillRoomHoles(roomBounds, floor);
                    --passes;
                }
            }

            rooms.Add(new Room(roomBounds, floor));
        }

        return rooms;
    }

    private void FillRoomHoles(BoundsInt roomBounds, HashSet<Vector2Int> floor)
    {
        for (int x = roomBounds.min.x + generationParams.offset; x < roomBounds.max.x - generationParams.offset; ++x)
        {
            for (int y = roomBounds.min.y + generationParams.offset; y < roomBounds.max.y - generationParams.offset; ++y)
            {
                var floorPos = new Vector2Int(x, y);
                if (floor.Contains(floorPos))
                {
                    // This is not a hole
                    continue;
                }

                // This is a hole, should it be filled?
                var directions = ProceduralGeneration.Direction2D.CardinalDirectionsList;
                int surroundingsCheck = 0;
                for (int dir = 0; dir < directions.Count; ++dir)
                {
                    if (floor.Contains(floorPos + directions[dir]))
                    {
                        surroundingsCheck |= 1 << dir;
                    }
                }

                if (shouldFillHole.Contains(surroundingsCheck))
                {
                    // Fill hole
                    floor.Add(floorPos);
                }
            }
        }
    }

    private List<List<Vector2Int>> ConnectRooms(List<Room> rooms, HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in rooms)
        {
            roomCenters.Add(room.center);
        }

        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
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

    private List<Room> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        List<Room> rooms = new List<Room>();
        foreach (var room in roomList)
        {
            HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
            for (int col = generationParams.offset; col < room.size.x - generationParams.offset; col++)
            {
                for (int row = generationParams.offset; row < room.size.y - generationParams.offset; row++)
                {
                    floor.Add((Vector2Int)room.min + new Vector2Int(col, row));
                }
            }

            rooms.Add(new Room(room, floor));
        }

        return rooms;
    }
}
