using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationParameters_", menuName = "PCG/DungeonGenerationData")]
public class DungeonGenerationData : ScriptableObject
{
    public DungeonType dungeonType;
    public SimpleRandomWalkData roomParams;
    public bool widenCorridors = true;
    public int minRoomWidth = 20;
    public int minRoomHeight = 20;
    public int dungeonWidth = 120;
    public int dungeonHeight = 120;
    [Range(0, 10)] public int offset = 1;
    public bool randomWalkRooms = false;
    public bool fillHoles = true;
    public bool generateCorridors = true;
    [Range(1, 10)] public int minRooms = 10;
    [Range(1, 15)] public int maxTries = 10;
}
