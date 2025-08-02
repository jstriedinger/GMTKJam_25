using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
    public Vector3Int spawnPosition = Vector3Int.zero;
    public Vector3Int instrumentPosition = Vector3Int.zero;

    public void Clear()
    {
        rooms.Clear();
        corridors.Clear();
    }
}
