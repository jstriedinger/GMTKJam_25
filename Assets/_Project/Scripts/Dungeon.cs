using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
}
