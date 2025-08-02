using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTilemapVisualizer : MonoBehaviour
{
    public DungeonType dungeonType = DungeonType.Tomb;

    public virtual void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
    }

    public virtual void PaintBasicWallTile(Vector2Int pos, int wallType)
    {
    }

    public virtual void PaintCornerWallTile(Vector2Int pos, int wallType)
    {
    }

    public virtual void Clear()
    {
    }
}
